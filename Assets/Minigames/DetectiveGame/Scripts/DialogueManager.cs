using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueContainer;
    public GameObject choicesContainer;
    public Button choiceButtonPrefab;
    public Button nextButton;

    private DialogueData currentDialogue;
    private int currentLineIndex;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (dialogueContainer != null)
            dialogueContainer.SetActive(false);
    }

    public void StartDialogue(DialogueData data)
    {
        if (dialogueContainer != null)
            dialogueContainer.SetActive(true);

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
        speakerText.text = line.speaker;
        dialogueText.text = line.text;

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
    }

    public void OnNextPressed()
    {
        var line = currentDialogue.lines[currentLineIndex];

        int? indexJump = line.nextLineIndex;
        if (indexJump != null)
            currentLineIndex = (int)indexJump;
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
    }
}