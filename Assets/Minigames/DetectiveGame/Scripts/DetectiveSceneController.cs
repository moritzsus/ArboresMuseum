using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetectiveSceneController : MonoBehaviour
{
    public static DetectiveSceneController Instance { get; private set; }

    public Image backgroundImage;
    public Transform hotspotContainer;
    public Transform characterContainer;

    public GameObject inspectOverlay;
    public Image inspectImage;
    public TextMeshProUGUI inspectText;
    public Button closeInspectButton;

    public RoomData[] rooms;

    [SerializeField]
    private GameObject hotspotPrefab;
    [SerializeField]
    private GameObject characterPrefab;
    [SerializeField]
    private Sprite defaultPlayerIcon;

    private Dictionary<string, RoomData> roomMap;
    private GameObject hotspotToRemoveAfterInspect;
    private GameObject museumStartExitHotspot;
    private GameObject policeRoomExitHotspot;
    private GameObject officerMarkerHotspot;

    private bool officerAlternativeDialogueSeen = false;
    private bool guardDialogueSeen = false;
    private int cluesFound = 0;
    private const int CLUES_NEEDED = 5;

    private HashSet<string> discoveredClues = new();
    private Dictionary<string, Sprite> dialogueIcons = new();

    private string currentRoomName;
    private List<CharacterData> currentCharacters = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        closeInspectButton.onClick.AddListener(CloseInspect);
        inspectOverlay.SetActive(false);

        roomMap = rooms.ToDictionary(r => r.name, r => r);
        dialogueIcons["Spieler"] = defaultPlayerIcon;
        LoadRoom("MuseumStart");
    }

    public bool IsCurrentRoom(string name)
    {
        return currentRoomName == name;
    }

    public bool IsSuspect(string name)
    {
        return currentCharacters.Any(c => c.name == name);
    }

    public bool IsGuilty(string name)
    {
        return currentCharacters.FirstOrDefault(c => c.name == name)?.isGuilty ?? false;
    }

    public int GetCluesCount()
    {
        return cluesFound;
    }

    public bool HasFoundStolenPainting()
    {
        return discoveredClues.Contains("HiddenImage");
    }

    public CharacterData GetCharacterDataByName(string name)
    {
        foreach (var room in rooms)
        {
            foreach (var ch in room.characters)
            {
                if (ch.name == name)
                    return ch;
            }
        }
        return null;
    }


    public void RegisterClue(string clueId)
    {
        if (!discoveredClues.Contains(clueId))
        {
            discoveredClues.Add(clueId);
            cluesFound++;
        }
    }

    public void MarkOfficerAsInformed()
    {
        officerAlternativeDialogueSeen = true;
    }

    public void SetInteractionEnabled(bool isEnabled)
    {
        foreach (Transform hotspot in hotspotContainer)
        {
            if (hotspot.TryGetComponent<Button>(out var btn))
                btn.interactable = isEnabled;
        }

        foreach (Transform character in characterContainer)
        {
            if (character.TryGetComponent<Button>(out var btn))
                btn.interactable = isEnabled;
        }
    }

    public void LoadRoom(string roomName)
    {
        currentRoomName = roomName;
        currentCharacters.Clear();

        RoomData room = roomMap[roomName];
        backgroundImage.sprite = room.backgroundImage;

        ClearHotspots();
        ClearCharacters();

        foreach (var hs in room.hotspots)
        {
            bool wasAlreadyFound = discoveredClues.Contains(hs.name);
            bool shouldRemove = hs.removeAfterFound && wasAlreadyFound;

            if (!shouldRemove)
            {
                CreateHotspot(hs);
            }
        }

        foreach (var ch in room.characters)
        {
            // Only create suspects after enough clues found
            if (roomName == "PoliceRoom" && cluesFound < CLUES_NEEDED)
                continue;

            CreateCharacter(ch);
            currentCharacters.Add(ch);
        }

        DialogueManager.Instance.SetSpeakerIcons(dialogueIcons);
    }

    private void CreateHotspot(HotspotData hotspotData)
    {
        GameObject hs = Instantiate(hotspotPrefab, hotspotContainer);

        RectTransform rt = hs.GetComponent<RectTransform>();
        rt.anchoredPosition = hotspotData.position;

        rt.localEulerAngles = new Vector3(0f, 0f, hotspotData.rotation);
        rt.sizeDelta = new Vector2(hotspotData.iconWidth, hotspotData.iconHeight);

        var button = hs.GetComponent<Button>();
        var image = hs.GetComponent<Image>();

        if (button != null)
        {
            ColorBlock cb = button.colors;
            cb.disabledColor = cb.normalColor;
            button.colors = cb;
        }

        image.sprite = hotspotData.icon;

        if (hotspotData.name == "Marker")
        {
            officerMarkerHotspot = hs;
            bool shouldShowMarker = GetCluesCount() >= CLUES_NEEDED && !officerAlternativeDialogueSeen;
            hs.SetActive(shouldShowMarker);
        }

        if (hotspotData.type == HotspotType.Exit && hotspotData.linkedRoomName == "PoliceRoom")
        {
            policeRoomExitHotspot = hs;

            // default: deactivate
            if (!officerAlternativeDialogueSeen)
                hs.SetActive(false);
        }

        if (hotspotData.type == HotspotType.Exit && hotspotData.name == "Move_Outside")
        {
            museumStartExitHotspot = hs;

            // default: deactivate
            if (!guardDialogueSeen)
                hs.SetActive(false);
        }

        switch (hotspotData.type)
        {
            case HotspotType.Dialogue:
                button.onClick.AddListener(() =>
                {
                    if (hotspotData.dialogFile != null)
                    {
                        DialogueData dialogue = JsonUtility.FromJson<DialogueData>(hotspotData.dialogFile.text);
                        DialogueManager.Instance.StartDialogue(dialogue);
                    }
                });
                break;
            case HotspotType.Inspect:
                button.onClick.AddListener(() => Inspect(hotspotData, hs));
                break;
            case HotspotType.Exit:
                button.onClick.AddListener(() => LoadRoom(hotspotData.linkedRoomName));
                break;
        }
    }

    private void CreateCharacter(CharacterData ch)
    {
        GameObject charObj = Instantiate(characterPrefab, characterContainer);

        RectTransform rt = charObj.GetComponent<RectTransform>();
        rt.anchoredPosition = ch.position;
        rt.sizeDelta = ch.size;

        Image image = charObj.GetComponent<Image>();
        image.sprite = ch.sprite;
        image.preserveAspect = true;
        image.type = Image.Type.Simple;

        Button button = charObj.GetComponent<Button>();
        if (button != null)
        {
            ColorBlock cb = button.colors;
            cb.disabledColor = cb.normalColor;
            button.colors = cb;
        }

        if (!dialogueIcons.ContainsKey(ch.name) && ch.characterDialogIcon != null)
            dialogueIcons.Add(ch.name, ch.characterDialogIcon);

        if (ch.dialogueFile != null)
        {
            button.onClick.RemoveAllListeners();

            if (ch.name == "Officer")
            {
                button.onClick.AddListener(() =>
                {
                    TextAsset selected = Instance.GetCluesCount() < CLUES_NEEDED
                        ? ch.dialogueFile
                        : ch.alternateDialogueFile;

                    if (selected != null)
                    {
                        DialogueData data = JsonUtility.FromJson<DialogueData>(selected.text);
                        DialogueManager.Instance.StartDialogue(data);

                        if (Instance.GetCluesCount() >= CLUES_NEEDED)
                        {
                            Instance.MarkOfficerAsInformed();

                            if (Instance.officerMarkerHotspot != null)
                                Instance.officerMarkerHotspot.SetActive(false);

                            if (policeRoomExitHotspot != null)
                                policeRoomExitHotspot.SetActive(true);
                        }
                    }
                });
            }
            else
            {
                button.onClick.AddListener(() =>
                {
                    DialogueData data = JsonUtility.FromJson<DialogueData>(ch.dialogueFile.text);
                    DialogueManager.Instance.StartDialogue(data);

                    if (ch.isClue)
                        Instance.RegisterClue(ch.name);

                    if (ch.name == "Wächter")
                    {
                        guardDialogueSeen = true;
                        if (museumStartExitHotspot != null)
                            museumStartExitHotspot.SetActive(true);
                    }
                });
            }
        }
        else
        {
            button.interactable = false;
        }
    }

    private void ClearHotspots()
    {
        foreach (Transform child in hotspotContainer)
            Destroy(child.gameObject);
    }

    private void ClearCharacters()
    {
        foreach (Transform child in characterContainer)
            Destroy(child.gameObject);
    }

    private void Inspect(HotspotData hotspot, GameObject sourceObj)
    {
        inspectOverlay.SetActive(true);

        RegisterClue(hotspot.name);

        inspectImage.sprite = hotspot.inspectImage;
        inspectText.text = hotspot.inspectText;

        if (hotspot.removeAfterFound)
        {
            hotspotToRemoveAfterInspect = sourceObj;
        }
    }

    private void CloseInspect()
    {
        inspectOverlay.SetActive(false);

        if (hotspotToRemoveAfterInspect != null)
        {
            Destroy(hotspotToRemoveAfterInspect);
            hotspotToRemoveAfterInspect = null;
        }
    }
}
