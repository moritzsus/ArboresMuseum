using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchingHandler : MonoBehaviour
{
    public Button[] imageButtons;
    public Button[] textButtons;

    private bool[] matchedImages = new bool[4];
    private bool[] matchedTexts = new bool[4];

    private int selectedImageIndex = -1;
    private int selectedTextIndex = -1;

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

        selectedImageIndex = -1;
        selectedTextIndex = -1;

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

    private void OnImageSelected(int imageIndex)
    {
        if (matchedImages[imageIndex]) return;

        if (selectedImageIndex == imageIndex)
        {
            SetButtonColor(imageButtons[imageIndex], Color.white);
            selectedImageIndex = -1;
            return;
        }

        if (selectedTextIndex != -1)
        {
            CheckMatch(imageIndex, selectedTextIndex);
            return;
        }

        if (selectedImageIndex != -1)
        {
            SetButtonColor(imageButtons[selectedImageIndex], Color.white);
        }

        selectedImageIndex = imageIndex;
        selectedTextIndex = -1;

        SetButtonColor(imageButtons[imageIndex], Color.yellow);
    }

    private void OnTextSelected(int textIndex)
    {
        if (matchedTexts[textIndex]) return;

        if (selectedTextIndex == textIndex)
        {
            SetButtonColor(textButtons[textIndex], Color.white);
            selectedTextIndex = -1;
            return;
        }

        if (selectedImageIndex != -1)
        {
            CheckMatch(selectedImageIndex, textIndex);
            return;
        }

        if (selectedTextIndex != -1)
        {
            SetButtonColor(textButtons[selectedTextIndex], Color.white);
        }

        selectedTextIndex = textIndex;
        selectedImageIndex = -1;

        SetButtonColor(textButtons[textIndex], Color.yellow);
    }

    private void CheckMatch(int imageIndex, int textIndex)
    {
        bool isCorrect = currentQuestion.correctMatches[imageIndex] == textIndex;

        Button imageBtn = imageButtons[imageIndex];
        Button textBtn = textButtons[textIndex];

        if (isCorrect)
        {
            SetButtonColor(imageBtn, Color.green);
            SetButtonColor(textBtn, Color.green);
            imageBtn.interactable = false;
            textBtn.interactable = false;

            matchedImages[imageIndex] = true;
            matchedTexts[textIndex] = true;

            selectedImageIndex = -1;
            selectedTextIndex = -1;

            ResetUnmatchedButtonColors();
        }
        else
        {
            StartCoroutine(FlashRed(imageBtn, textBtn));
        }

        if (AllMatched())
        {
            quizManager.EnableNextButton();
        }
    }

    private IEnumerator FlashRed(Button imageBtn, Button textBtn)
    {
        Debug.Log("Red");

        SetButtonColor(imageBtn, Color.red);
        SetButtonColor(textBtn, Color.red);

        yield return new WaitForSeconds(1f);

        if (!matchedImages[Array.IndexOf(imageButtons, imageBtn)])
            SetButtonColor(imageBtn, Color.white);
        if (!matchedTexts[Array.IndexOf(textButtons, textBtn)])
            SetButtonColor(textBtn, Color.white);

        selectedImageIndex = -1;
        selectedTextIndex = -1;

        ResetUnmatchedButtonColors();
    }

    private void SetButtonColor(Button btn, Color color)
    {
        ColorBlock cb = btn.colors;
        cb.normalColor = color;
        cb.highlightedColor = color;
        cb.pressedColor = color;
        cb.selectedColor = color;
        cb.disabledColor = color;
        btn.colors = cb;
    }

    private void ResetUnmatchedButtonColors()
    {
        for (int i = 0; i < 4; i++)
        {
            if (!matchedImages[i]) SetButtonColor(imageButtons[i], Color.white);
            if (!matchedTexts[i]) SetButtonColor(textButtons[i], Color.white);
        }
    }

    private bool AllMatched()
    {
        return matchedImages.All(m => m);
    }
}
