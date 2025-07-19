using System.Collections.Generic;
using UnityEngine;

public static class QuestionHelper
{
    static readonly Sprite _multiple1 = Resources.Load<Sprite>("QuizGame/Multiple1_512");
    static readonly Sprite _multiple2 = Resources.Load<Sprite>("QuizGame/Multiple2_512");
    static readonly Sprite _multiple3 = Resources.Load<Sprite>("QuizGame/Multiple3_512");

    static readonly Sprite _matching1a = Resources.Load<Sprite>("QuizGame/Match1A512");
    static readonly Sprite _matching1b = Resources.Load<Sprite>("QuizGame/Match1B512");
    static readonly Sprite _matching1c = Resources.Load<Sprite>("QuizGame/Match1C512");
    static readonly Sprite _matching1d = Resources.Load<Sprite>("QuizGame/Match1D512");

    static readonly Sprite _matching2a = Resources.Load<Sprite>("QuizGame/Match2A512");
    static readonly Sprite _matching2b = Resources.Load<Sprite>("QuizGame/Match2B512");
    static readonly Sprite _matching2c = Resources.Load<Sprite>("QuizGame/Match2C512");
    static readonly Sprite _matching2d = Resources.Load<Sprite>("QuizGame/Match2D512");

    static readonly Sprite _matching3a = Resources.Load<Sprite>("QuizGame/Match3A512");
    static readonly Sprite _matching3b = Resources.Load<Sprite>("QuizGame/Match3B512");
    static readonly Sprite _matching3c = Resources.Load<Sprite>("QuizGame/Match3C512");
    static readonly Sprite _matching3d = Resources.Load<Sprite>("QuizGame/Match3D512");

    static readonly Sprite _trueFalse1 = Resources.Load<Sprite>("QuizGame/TF1_512");
    static readonly Sprite _trueFalse2 = Resources.Load<Sprite>("QuizGame/TF2_512");
    static readonly Sprite _trueFalse3 = Resources.Load<Sprite>("QuizGame/TF3_512");

    public static List<QuestionBase> InitQuestions()
    {
        return new List<QuestionBase>
        {
            new MultipleQuestion
            {
                questionType = QuestionType.Multiple,
                image = _multiple1,
                questionText = "Was ist das Thema des Werks, in dem dieses Bild vorkommt?",
                answers = new string[]
                {
                    "Ein Organisationsdokument zur militärischen und zivilen Struktur nach den Reformen unter Diocletian.",
                    "Tagebuch eines römischen Soldaten im Kampf gegen die Germanen.",
                    "Eine von Bischof Augustinus von Hippo erstellte Sammlung von religiösen Texten und Gebeten aus dem 5. Jahrhundert.",
                    "Ein Lehrbuch zur Tiermedizin aus dem 2. Jahrhundert nach Christus mit Fokus auf Nutztiere."
                },
                correctAnswerIndex = 0
            },
            new MultipleQuestion
            {
                questionType = QuestionType.Multiple,
                image = _multiple2,
                questionText = "Was ist das Thema des Werks, in dem dieses Bild vorkommt?",
                answers = new string[]
                {
                    "Ein Sammelwerk zur Zeitrechnung und Kalenderkonstruktion unter Berücksichtigung von Mathematik, Kosmologie, Medizin und Geschichte.",
                    "Ein astrologisches Handbuch zur Erstellung von Horoskopen für königliche und kirchliche Auftraggeber.",
                    "Ein philosophisches Werk zur Rechtfertigung weltlicher Alleinherrscher durch Bezug auf christliche Überlieferungen.",
                    "Ein nordafrikanisches illustriertes Herbarium mit botanischen Zeichnungen und Heilpflanzenrezepten."
                },
                correctAnswerIndex = 0
            },
            new MultipleQuestion
            {
                questionType = QuestionType.Multiple,
                image = _multiple3,
                questionText = "Was ist das Thema des Werks, in dem dieses Bild vorkommt?",
                answers = new string[]
                {
                    "Eine mittelalterliche Sammlung angelsächsischer Rechtstexte zur Regelung von Landbesitz, um den Adel gegenüber niedrigeren Schichten verstärkt zu bevorzugen.",
                    "Ein medizinisches Fachbuch zur Diagnose von Erkrankungen inklusive Entscheidungsbäumen auf Basis typischer Symptome.",
                    "Ein in Westdeutschland verfasstes Manuskript mit einer Kanonensammlung, ergänzt durch weitere theologische Texte und ein Verwandtschaftsdiagramm.",
                    "Eine Biografiensammlung heiliger Frauen aus dem Umfeld Karls des Großen."
                },
                correctAnswerIndex = 2
            },
            new MatchingQuestion
            {
                questionType = QuestionType.Matching,
                questionText = "Ordne die Bilder ihren Jahrhunderten zu.",
                images = new Sprite[] { _matching1a, _matching1b, _matching1c, _matching1d },
                answerTexts = new string[] { "9. Jahrhundert", "13. Jahrhundert", "16. Jahrhundert", "21. Jahrhundert" },
                correctMatches = new int[] { 0, 1, 2, 3 }
            },
            new MatchingQuestion
            {
                questionType = QuestionType.Matching,
                questionText = "Ordne die Bilder dem jeweiligen Werk in dem sie vorkommen zu.",
                images = new Sprite[] { _matching2a, _matching2b, _matching2c, _matching2d },
                answerTexts = new string[] { "Commentary on the Apocalypse", "Notitia Dignitatum", "Somme rurale", "Tractatus de arboribus consanguinitatis et affinitatis" },
                correctMatches = new int[] { 0, 1, 2, 3 }
            },
            new MatchingQuestion
            {
                questionType = QuestionType.Matching,
                questionText = "Ordne die Bilder ihrem aktuellen Aufbewahrungsort zu.",
                images = new Sprite[] { _matching3a, _matching3b, _matching3c, _matching3d },
                answerTexts = new string[] { "Wien, Österreichische Nationalbibliothek", "Paris, Louvre", "Brüssel, Bibliothèqzue Royale", "Leipzip, Universitätsbibliothek" },
                correctMatches = new int[] { 0, 1, 2, 3 }
            },
            new TrueFalseQuestion
            {
                questionType = QuestionType.TrueFalse,
                image = _trueFalse1,
                questionText = "Die Erde ist eine Scheibe.",
                correctAnswer = false
            },
            new TrueFalseQuestion
            {
                questionType = QuestionType.TrueFalse,
                image = _trueFalse2,
                questionText = "Wasser kocht bei 100°C.",
                correctAnswer = true
            },
            new TrueFalseQuestion
            {
                questionType = QuestionType.TrueFalse,
                image = _trueFalse3,
                questionText = "Tauben sind Spione vom Staat.",
                correctAnswer = true
            }
        };
    }
}