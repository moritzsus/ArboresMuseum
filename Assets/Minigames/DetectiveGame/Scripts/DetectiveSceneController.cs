using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetectiveSceneController : MonoBehaviour
{
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

    private GameObject hotspotToRemoveAfterInspect;
    private Dictionary<string, RoomData> roomMap;

    void Start()
    {
        //closeInspectButton.onClick.AddListener(() => inspectOverlay.SetActive(false));
        closeInspectButton.onClick.AddListener(CloseInspect);
        inspectOverlay.SetActive(false);

        roomMap = rooms.ToDictionary(r => r.name, r => r);
        LoadRoom("MuseumStart");
    }

    public void LoadRoom(string roomName)
    {
        RoomData room = roomMap[roomName];
        backgroundImage.sprite = room.backgroundImage;

        ClearHotspots();
        ClearCharacters();

        foreach (var hs in room.hotspots)
            CreateHotspot(hs);

        foreach (var ch in room.characters)
            CreateCharacter(ch);
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
        image.sprite = hotspotData.icon;

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
        if (ch.dialogueFile != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                DialogueData data = JsonUtility.FromJson<DialogueData>(ch.dialogueFile.text);
                DialogueManager.Instance.StartDialogue(data);
            });
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
