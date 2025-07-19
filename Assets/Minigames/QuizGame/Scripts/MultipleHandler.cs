using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultipleHandler : MonoBehaviour
{
    public Image image;
    public Button[] answerButtons;
    private int correctAnswerIndex;
    private QuizManager quizManager;

    private void Start()
    {
        quizManager = GetComponent<QuizManager>();
    }

    public void Setup(MultipleQuestion question)
    {
        correctAnswerIndex = question.correctAnswerIndex;

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

            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = question.answers[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
        }

        if (image != null)
            image.sprite = question.image;
    }

    private void OnAnswerSelected(int selectedIndex)
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].interactable = false;

            ColorBlock cb = answerButtons[i].colors;

            if (i == correctAnswerIndex)
                cb.disabledColor = Color.green;
            else if (i == selectedIndex)
                cb.disabledColor = Color.red;

            answerButtons[i].colors = cb;
        }

        Debug.Log("Befor next");
        quizManager.EnableNextButton();
    }
}