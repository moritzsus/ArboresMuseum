using UnityEngine;

[System.Serializable]
public class QuestionBase
{
    public QuestionType questionType;
    public string questionText;
}

[System.Serializable]
public class MultipleQuestion : QuestionBase
{
    public Sprite image;
    public string[] answers;
    public int correctAnswerIndex;
}

[System.Serializable]
public class MatchingQuestion : QuestionBase
{
    public Sprite[] images;
    public string[] answerTexts;
    public int[] correctMatches;
}

[System.Serializable]
public class TrueFalseQuestion : QuestionBase
{
    public Sprite image;
    public bool correctAnswer;
    public string infoText;
}
