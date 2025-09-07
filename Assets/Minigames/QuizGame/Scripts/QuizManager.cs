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
    [SerializeField] private TextMeshProUGUI infoText;

    [Header("UI References")]
    public TextMeshProUGUI questionText;
    public Button nextButton;

    [Header("Questions")]
    public List<QuestionBase> questions;

    private int currentQuestionIndex = 0;
    private int totalScore = 100;
    private Dictionary<int, int> matchingErrorCounts = new Dictionary<int, int>();

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
            ShowResults();
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
                if (!matchingErrorCounts.ContainsKey(currentQuestionIndex))
                {
                    matchingErrorCounts[currentQuestionIndex] = 0;
                }
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

    public void DeductPointsForMultipleChoice()
    {
        totalScore = Mathf.Max(0, totalScore - 6);
    }

    public void DeductPointsForMatching()
    {
        // limit max point deduction
        if (matchingErrorCounts[currentQuestionIndex] < 3)
        {
            matchingErrorCounts[currentQuestionIndex]++;
            totalScore = Mathf.Max(0, totalScore - 4);
        }
    }

    public void DeductPointsForTrueFalse()
    {
        totalScore = Mathf.Max(0, totalScore - 10);
    }

    private void ShowResults()
    {
        endUiCanvas.SetActive(true);

        if (infoText != null)
        {
            infoText.text = $"Du hast alle Fragen beantwortet. Dabei hast du {totalScore} von 100 Punkten erreicht.";
        }

        GameSettings.Instance.MarkMinigameCompleted(2);
    }
}
