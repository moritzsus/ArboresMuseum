using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchingHandler : MonoBehaviour
{
    public Button[] imageButtons;
    public Button[] textButtons;
    private SpriteState defaultSpriteState;

    private int selectedImageIndex = -1;
    private bool[] matchedImages = new bool[4];
    private bool[] matchedTexts = new bool[4];
    private MatchingQuestion currentQuestion;
    private QuizManager quizManager;

    void Start()
    {
        quizManager = GetComponent<QuizManager>();
    }

    public void Setup(MatchingQuestion question)
    {
        currentQuestion = question;
        selectedImageIndex = -1;
        matchedImages = new bool[4];
        matchedTexts = new bool[4];

        for (int i = 0; i < 4; i++)
        {
            int index = i;
            imageButtons[i].image.sprite = question.images[i];
            imageButtons[i].interactable = true;
            imageButtons[i].onClick.RemoveAllListeners();
            imageButtons[i].onClick.AddListener(() => OnImageSelected(index));

            textButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = question.answerTexts[i];
            textButtons[i].interactable = true;
            textButtons[i].onClick.RemoveAllListeners();
            textButtons[i].onClick.AddListener(() => OnTextSelected(index));

            SetButtonColor(imageButtons[i], Color.white);
            SetButtonColor(textButtons[i], Color.white);
        }
    }

    private void OnImageSelected(int index)
    {
        if (matchedImages[index]) return;

        selectedImageIndex = index;

        for (int i = 0; i < 4; i++)
            SetButtonColor(imageButtons[i], matchedImages[i] ? Color.green : Color.white);

        SetButtonColor(imageButtons[index], Color.yellow);
    }

    private void OnTextSelected(int textIndex)
    {
        if (selectedImageIndex == -1 || matchedTexts[textIndex]) return;

        int correctTextIndex = currentQuestion.correctMatches[selectedImageIndex];
        Button imageBtn = imageButtons[selectedImageIndex];
        Button textBtn = textButtons[textIndex];

        if (textIndex == correctTextIndex)
        {
            SetButtonColor(imageBtn, Color.green);
            SetButtonColor(textBtn, Color.green);
            imageBtn.interactable = false;
            textBtn.interactable = false;
            matchedImages[selectedImageIndex] = true;
            matchedTexts[textIndex] = true;
        }
        else
        {
            StartCoroutine(FlashRed(imageBtn, textBtn));
        }

        selectedImageIndex = -1;

        if (AllMatched())
        {
            quizManager.EnableNextButton();
        }
    }

    private IEnumerator FlashRed(Button imageBtn, Button textBtn)
    {
        SetButtonColor(imageBtn, Color.red);
        SetButtonColor(textBtn, Color.red);
        yield return new WaitForSeconds(1f);
        SetButtonColor(imageBtn, Color.white);
        SetButtonColor(textBtn, Color.white);
    }

    private bool AllMatched()
    {
        return matchedImages.All(m => m);
    }

    private void SetButtonColor(Button btn, Color color)
    {
        ColorBlock cb = btn.colors;
        cb.normalColor = color;
        cb.disabledColor = color;
        btn.colors = cb;
    }
}
