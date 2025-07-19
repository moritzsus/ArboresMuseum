using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrueFalseHandler : MonoBehaviour
{
    public Image image;
    public Button trueButton;
    public Button falseButton;
    public TextMeshProUGUI infoText;

    private bool correctAnswer;
    private QuizManager quizManager;

    void Start()
    {
        quizManager = GetComponent<QuizManager>();
    }

    public void Setup(TrueFalseQuestion question)
    {
        correctAnswer = question.correctAnswer;

        trueButton.interactable = true;
        falseButton.interactable = true;

        ResetButtonColors();

        trueButton.onClick.RemoveAllListeners();
        falseButton.onClick.RemoveAllListeners();

        trueButton.onClick.AddListener(() => OnAnswerSelected(true, question.infoText));
        falseButton.onClick.AddListener(() => OnAnswerSelected(false, question.infoText));

        if (image != null)
            image.sprite = question.image;

        if (infoText != null)
            infoText.text = "";
    }

    private void OnAnswerSelected(bool selectedAnswer, string info)
    {
        trueButton.interactable = false;
        falseButton.interactable = false;

        if (selectedAnswer == correctAnswer)
        {
            SetButtonColor(selectedAnswer ? trueButton : falseButton, Color.green);
        }
        else
        {
            SetButtonColor(selectedAnswer ? trueButton : falseButton, Color.red);
            SetButtonColor(!selectedAnswer ? trueButton : falseButton, Color.green);
        }

        if (infoText != null)
            infoText.text = info;

        quizManager.EnableNextButton();
    }

    private void SetButtonColor(Button button, Color color)
    {
        var cb = button.colors;
        cb.disabledColor = color;
        button.colors = cb;
    }

    private void ResetButtonColors()
    {
        SetButtonColor(trueButton, Color.white);
        SetButtonColor(falseButton, Color.white);
    }
}
