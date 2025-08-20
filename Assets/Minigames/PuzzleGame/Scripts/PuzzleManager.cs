using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    public GameObject puzzlePiecePrefab;
    public RectTransform puzzleParent;
    public GameObject endUiCanvas;

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

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        endUiCanvas.SetActive(false);

        spritePath = spritePaths[Random.Range(0, spritePaths.Count)];

        LoadSprites();
        GeneratePuzzlePieces();

    }

    public void NotifyPieceCorrect()
    {
        correctPieces++;

        if (correctPieces >= totalPieces)
        {
            Debug.Log("Won");

            GameSettings.Instance.MarkMinigameCompleted(0);

            endUiCanvas.SetActive(true);
            //SceneManager.LoadScene("Museum", LoadSceneMode.Single);
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
        Shuffle(spriteIndices);

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

    private void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
