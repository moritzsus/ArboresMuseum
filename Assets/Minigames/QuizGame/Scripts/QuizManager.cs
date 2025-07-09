using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public Button nextButton;

    [Header("Questions")]
    public Question[] questions;

    private int currentQuestionIndex = 0;
    private bool answered = false;

    void Start()
    {
        nextButton.interactable = false;
        LoadQuestion();
    }

    void Update()
    {

    }

    public void OnNextQuestion()
    {
        Debug.Log("Next");

        currentQuestionIndex++;

        if (currentQuestionIndex >= questions.Length)
        {
            Debug.Log("Quiz done!");
            return;
        }

        LoadQuestion();
    }

    private void LoadQuestion()
    {
        answered = false;
        nextButton.interactable = false;

        Question q = questions[currentQuestionIndex];
        questionText.text = q.questionText;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i; // local var for listener

            answerButtons[i].interactable = true;

            ColorBlock cb = answerButtons[i].colors;
            cb.normalColor = Color.white;
            cb.highlightedColor = new Color(0.8f, 0.8f, 0.8f);
            cb.pressedColor = Color.gray;
            cb.selectedColor = Color.white;
            cb.disabledColor = new Color(0.7f, 0.7f, 0.7f);
            answerButtons[i].colors = cb;

            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = q.answers[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
        }
    }

    private void OnAnswerSelected(int selectedIndex)
    {
        Debug.Log("Answer selected: " + selectedIndex);

        if (answered)
            return;

        answered = true;

        Question q = questions[currentQuestionIndex];

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].interactable = false;

            ColorBlock cb = answerButtons[i].colors;

            if (i == q.correctAnswerIndex)
                cb.disabledColor = Color.green;
            else if (i == selectedIndex)
                cb.disabledColor = Color.red;

            //cb.normalColor = (i == q.correctAnswerIndex)
            //    ? Color.green
            //    : (i == selectedIndex ? Color.red : cb.normalColor);

            answerButtons[i].colors = cb;
        }

        nextButton.interactable = true;
    }
}
