using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public GameObject endUiCanvas;
    public TextMeshProUGUI endTitle;
    public TextMeshProUGUI endText;

    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueContainer;
    public GameObject choicesContainer;
    public Button choiceButtonPrefab;
    public Button nextButton;
    public Button accuseButton;
    public Image dialogueIconImage;

    private DialogueData currentDialogue;
    private int currentLineIndex;

    private Dictionary<string, Sprite> speakerIcons = new();
    private string playerDisplayName;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (dialogueContainer != null)
            dialogueContainer.SetActive(false);
    }

    private void Start()
    {
        endUiCanvas.SetActive(false);
    }

    public void SetSpeakerIcons(Dictionary<string, Sprite> iconMap)
    {
        speakerIcons = iconMap;
    }

    public void StartDialogue(DialogueData data)
    {
        accuseButton.gameObject.SetActive(false);

        if (dialogueContainer != null)
            dialogueContainer.SetActive(true);

        DetectiveSceneController.Instance.SetInteractionEnabled(false);

        playerDisplayName = GameSettings.Instance.PlayerName;

        currentDialogue = data;
        currentLineIndex = 0;
        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        ClearChoices();

        if (currentLineIndex >= currentDialogue.lines.Length)
        {
            EndDialogue();
            return;
        }

        var line = currentDialogue.lines[currentLineIndex];

        string displaySpeaker = (line.speaker == "Spieler") ? playerDisplayName : line.speaker;
        string displayText = (line.text ?? string.Empty)
            .Replace("<Name>", playerDisplayName);

        speakerText.text = displaySpeaker;
        dialogueText.text = displayText;

        if (speakerIcons.TryGetValue(line.speaker, out Sprite icon))
        {
            dialogueIconImage.sprite = icon;
        }
        else
            dialogueIconImage.sprite = null;

        if (line.choices != null && line.choices.Length > 0)
        {
            nextButton.gameObject.SetActive(false);

            foreach (var choice in line.choices)
            {
                Button btn = Instantiate(choiceButtonPrefab, choicesContainer.transform);
                btn.GetComponentInChildren<TextMeshProUGUI>().text = choice.text;
                btn.onClick.AddListener(() => OnChoiceSelected(choice.nextLineIndex));
            }
        }
        else
        {
            nextButton.gameObject.SetActive(true);
        }

        // Show acuse button if we speak with suspects
        bool isInPoliceRoom = DetectiveSceneController.Instance.IsCurrentRoom("PoliceRoom");
        bool isSuspect = DetectiveSceneController.Instance.IsSuspect(line.speaker);

        if (isInPoliceRoom && isSuspect)
        {
            accuseButton.gameObject.SetActive(true);
            accuseButton.onClick.RemoveAllListeners();
            accuseButton.onClick.AddListener(() =>
            {
                DialogueManager.Instance.OnAccuse(line.speaker);
            });

            // Move acuse button over the current suspect
            CharacterData ch = DetectiveSceneController.Instance.GetCharacterDataByName(line.speaker);
            if (ch != null)
            {
                RectTransform rt = accuseButton.GetComponent<RectTransform>();
                Vector2 pos = rt.anchoredPosition;

                pos.x = ch.position.x + 800; // 800 = ca Layout offset

                rt.anchoredPosition = pos;
            }
        }
        else
        {
            accuseButton.gameObject.SetActive(false);
        }
    }

    public void OnNextPressed()
    {
        var line = currentDialogue.lines[currentLineIndex];
        int nextLine = line.nextLineIndex;
        if (nextLine != -1)
            currentLineIndex = nextLine;
        else
            currentLineIndex++;

        ShowCurrentLine();
    }

    private void OnChoiceSelected(int nextIndex)
    {
        currentLineIndex = nextIndex;
        ShowCurrentLine();
    }

    private void ClearChoices()
    {
        foreach (Transform child in choicesContainer.transform)
            Destroy(child.gameObject);
    }

    private void EndDialogue()
    {
        dialogueText.text = "";
        speakerText.text = "";
        nextButton.gameObject.SetActive(false);
        ClearChoices();

        if (dialogueContainer != null)
            dialogueContainer.SetActive(false);

        DetectiveSceneController.Instance.SetInteractionEnabled(true);
    }

    public void OnAccuse(string accusedName)
    {
        bool isCorrect = DetectiveSceneController.Instance.IsGuilty(accusedName);
        bool foundPainting = DetectiveSceneController.Instance.HasFoundStolenPainting();
        int cluesFound = DetectiveSceneController.Instance.GetCluesCount();

        EndDialogue();
        DetectiveSceneController.Instance.SetInteractionEnabled(false);

        GameSettings.Instance.MarkMinigameCompleted(3);

        endTitle.text = isCorrect || foundPainting ? "Glückwunsch" : "Spielende";

        string infoText = $"Du hast {cluesFound} Hinweise gefunden.\n";
        infoText += foundPainting ? "Du hast das gestohlene Bild gefunden.\n" : "Du hast das gestohlene Bild nicht gefunden.\n";
        infoText += isCorrect ? "Die identifizierte Person war der Täter." : "Die identifizierte Person war nicht der Täter.";

        endText.text = infoText;

        endUiCanvas.SetActive(true);
    }
}