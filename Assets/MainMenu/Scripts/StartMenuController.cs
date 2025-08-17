using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    [Header("Modus (2 Buttons)")]
    [SerializeField] private Button playModeButton;
    [SerializeField] private Button exploreButton;
    [SerializeField] private Color activeButtonColor = new Color(0.30f, 0.75f, 1f, 1f);
    [SerializeField] private Color inactiveButtonColor = Color.white;

    [Header("Name")]
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private int minNameLen = 2;
    [SerializeField] private int maxNameLen = 14;

    [Header("Charaktere")]
    [SerializeField] private CharacterSelectButton[] characterButtons;
    [SerializeField] private int defaultCharacterIndex = 0;

    [Header("Abschnitte (Root-GameObjects)")]
    [SerializeField] private GameObject nameSectionRoot;
    [SerializeField] private GameObject charactersSectionRoot;

    [Header("Flow")]
    [SerializeField] private Button startButton;
    [SerializeField] private string museumSceneName = "Museum";

    private GameMode currentMode = GameMode.Explore;
    private int selectedIndex;

    private void Awake()
    {
        // Falls Szene direkt gestartet wird: GameSettings sicherstellen
        if (!GameSettings.Instance)
        {
            var go = new GameObject("GameSettings");
            go.AddComponent<GameSettings>();
        }

        selectedIndex = Mathf.Clamp(defaultCharacterIndex, 0, characterButtons.Length - 1);
        if (nameInput) nameInput.characterLimit = maxNameLen;

        // Anfangszustand UI
        SetMode(GameMode.Play, updateUIOnly: true);
        UpdateCharacterHighlights();
        UpdateValidationUI();

        // Listeners
        if (nameInput) nameInput.onValueChanged.AddListener(_ => UpdateValidationUI());
        foreach (var btn in characterButtons)
            if (btn) btn.SetSelected(btn.Index == selectedIndex);
    }

    private void Start()
    {
        SelectCharacter(0);
    }

    // Diese beiden Funktionen im Inspector auf die Buttons legen:
    public void OnClickPlay() => SetMode(GameMode.Play);
    public void OnClickExplore() => SetMode(GameMode.Explore);

    private void SetMode(GameMode mode, bool updateUIOnly = false)
    {
        currentMode = mode;

        // Button-Farben + Highlights
        if (exploreButton && exploreButton.targetGraphic)
            (exploreButton.targetGraphic as Graphic).color =
                (mode == GameMode.Explore) ? activeButtonColor : inactiveButtonColor;

        if (playModeButton && playModeButton.targetGraphic)
            (playModeButton.targetGraphic as Graphic).color =
                (mode == GameMode.Play) ? activeButtonColor : inactiveButtonColor;

        bool showPlayUI = mode == GameMode.Play;
        if (nameSectionRoot) nameSectionRoot.SetActive(showPlayUI);
        if (charactersSectionRoot) charactersSectionRoot.SetActive(showPlayUI);

        if (!updateUIOnly) UpdateValidationUI();
    }

    public void SelectCharacter(int index)
    {
        index = Mathf.Clamp(index, 0, characterButtons.Length - 1);
        if (selectedIndex == index) return;
        selectedIndex = index;
        UpdateCharacterHighlights();
        UpdateValidationUI();
    }

    private void UpdateCharacterHighlights()
    {
        for (int i = 0; i < characterButtons.Length; i++)
            if (characterButtons[i])
                characterButtons[i].SetSelected(i == selectedIndex);
    }

    private bool IsNameValid(string n)
    {
        if (string.IsNullOrWhiteSpace(n)) return false;
        n = n.Trim();
        return n.Length >= minNameLen && n.Length <= maxNameLen;
    }

    private void UpdateValidationUI()
    {
        string n = nameInput ? nameInput.text : "";
        bool needName = currentMode == GameMode.Play;
        bool ok = !needName || IsNameValid(n);

        if (startButton) startButton.interactable = ok;
    }

    public void OnClickStart()
    {
        string n = nameInput ? nameInput.text.Trim() : "";
        if (currentMode == GameMode.Play && !IsNameValid(n))
        {
            UpdateValidationUI();
            return;
        }

        GameSettings.Instance.Apply(currentMode, n, selectedIndex);
        SceneManager.LoadScene(museumSceneName, LoadSceneMode.Single);
    }
}
