using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public GameObject answerGroupMultiple;
    public GameObject answerGroupMatching;
    public GameObject answerGroupTrueFalse;

    public GameObject endUiCanvas;

    [Header("UI References")]
    public TextMeshProUGUI questionText;
    public Button nextButton;

    [Header("Questions")]
    public List<QuestionBase> questions;

    private int currentQuestionIndex = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        endUiCanvas.SetActive(false);

        questions = QuestionHelper.InitQuestions();
        nextButton.interactable = false;
        LoadQuestion();
    }

    public void OnNextQuestion()
    {
        currentQuestionIndex++;

        if (currentQuestionIndex >= questions.Count)
        {
            endUiCanvas.SetActive(true);

            GameSettings.Instance.MarkMinigameCompleted(2);
            return;
        }

        LoadQuestion();
    }

    private void LoadQuestion()
    {
        nextButton.interactable = false;

        answerGroupMultiple.SetActive(false);
        answerGroupMatching.SetActive(false);
        answerGroupTrueFalse.SetActive(false);

        var q = questions[currentQuestionIndex];
        questionText.text = q.questionText;

        switch (q.questionType)
        {
            case QuestionType.Multiple:
                answerGroupMultiple.SetActive(true);
                var mq = (MultipleQuestion)q;
                GetComponent<MultipleHandler>().Setup(mq);
                break;

            case QuestionType.Matching:
                answerGroupMatching.SetActive(true);
                var matchQ = (MatchingQuestion)q;
                GetComponent<MatchingHandler>().Setup(matchQ);
                break;

            case QuestionType.TrueFalse:
                answerGroupTrueFalse.SetActive(true);
                var tfq = (TrueFalseQuestion)q;
                GetComponent<TrueFalseHandler>().Setup(tfq);
                break;
        }
    }

    public void EnableNextButton()
    {
        nextButton.interactable = true;
    }
}
