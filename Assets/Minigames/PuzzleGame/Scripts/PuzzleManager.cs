using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    public GameObject puzzlePiecePrefab;
    public RectTransform puzzleParent;
    public GameObject endUiCanvas;
    [SerializeField] private TextMeshProUGUI infoText;

    private string spritePath = "";
    private List<string> spritePaths = new()
    {
        "PuzzleGame/PuzzleArbor",
        "PuzzleGame/P931_Puzzle",
        "PuzzleGame/P441_Puzzle",
        "PuzzleGame/P1N1_Puzzle",
        "PuzzleGame/DO11_Puzzle",
        "PuzzleGame/CB31_Puzzle",
        "PuzzleGame/AU11_Puzzle"
    };

    private Sprite[] puzzleSprites;

    private int gridSize = 5;
    private float pieceWidth = 120f;
    private float pieceHeight = 192f;
    private int correctPieces = 0;
    private int totalPieces = 25;

    private float startTime;
    private bool timerRunning = false;
    private bool isPaused = false;
    private float totalPauseTime = 0f;
    private float pauseStartTime = 0f;

    private int gameScore = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        endUiCanvas.SetActive(false);

        spritePath = spritePaths[Random.Range(0, spritePaths.Count)];

        LoadSprites();
        GeneratePuzzlePieces();
    }

    public void StartTimer()
    {
        startTime = Time.time;
        timerRunning = true;
        isPaused = false;
        totalPauseTime = 0f;
    }

    public void PauseTimer()
    {
        if (timerRunning && !isPaused)
        {
            isPaused = true;
            pauseStartTime = Time.time;
        }
    }

    public void ResumeTimer()
    {
        if (timerRunning && isPaused)
        {
            isPaused = false;
            totalPauseTime += Time.time - pauseStartTime;
        }
    }

    private void StopTimer()
    {
        if (timerRunning)
        {
            float elapsedTime = Time.time - startTime - totalPauseTime;
            timerRunning = false;

            gameScore = CalculateScore(elapsedTime);

            endUiCanvas.SetActive(true);

            infoText.text = $"Du hast das Arbor-Bild erfolgreich zusammen gesetzt.\n" +
                           $"Dafür hast du {elapsedTime:F1} Sekunden gebraucht und {gameScore} Punkte bekommen.\n" +
                           $"Kehre nun zurück zum Museum oder spiele erneut.";
        }
    }

    public void NotifyPieceCorrect()
    {
        correctPieces++;

        if (correctPieces >= totalPieces)
        {
            StopTimer();

            GameSettings.Instance.MarkMinigameCompleted(0, gameScore);
        }
    }

    private void LoadSprites()
    {
        puzzleSprites = Resources.LoadAll<Sprite>(spritePath);
    }

    private void GeneratePuzzlePieces()
    {
        List<Vector2> targetPositions = new List<Vector2>();
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                float posX = x * pieceWidth;
                float posY = -y * pieceHeight;

                targetPositions.Add(new Vector2(posX, posY));
            }
        }

        List<int> spriteIndices = Enumerable.Range(0, puzzleSprites.Length).ToList();

        for (int i = 0; i < puzzleSprites.Length; i++)
        {
            GameObject piece = Instantiate(puzzlePiecePrefab, puzzleParent);
            RectTransform rt = piece.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(pieceWidth, pieceHeight);

            int spriteIndex = spriteIndices[i];
            piece.GetComponent<Image>().sprite = puzzleSprites[spriteIndex];

            Vector2 targetPos = targetPositions[spriteIndex];
            piece.GetComponent<PuzzlePiece>().Init(targetPos, spriteIndex);

            rt.anchoredPosition = new Vector2(
                Random.Range(700f, 1500f),
                Random.Range(0, -720f)
            );
        }
    }

    private int CalculateScore(float timeInSeconds)
    {
        // faster than 45 seconds => max points
        if (timeInSeconds <= 45f)
        {
            return 100;
        }

        // subtract a point every 2 seconds
        int deduction = Mathf.FloorToInt(((timeInSeconds - 45f) / 2f) + 1);
        int score = 100 - deduction;

        return Mathf.Max(0, score);
    }
}
