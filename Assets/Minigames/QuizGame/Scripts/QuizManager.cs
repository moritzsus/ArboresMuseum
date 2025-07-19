using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public GameObject answerGroupMultiple;
    public GameObject answerGroupMatching;
    public GameObject answerGroupTrueFalse;

    [Header("UI References")]
    public TextMeshProUGUI questionText;
    public Button nextButton;

    [Header("Questions")]
    public List<QuestionBase> questions;

    private int currentQuestionIndex = 0;

    void Start()
    {
        questions = QuestionHelper.InitQuestions();
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

        if (currentQuestionIndex >= questions.Count)
        {
            Debug.Log("Quiz done!");
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
