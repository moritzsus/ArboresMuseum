using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzlePauseUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject root;
    [SerializeField] private Button museumButton;
    [SerializeField] private Button continueButton;

    [SerializeField] private PuzzleManager puzzleManager;

    private bool isPaused = false;

    private void Start()
    {
        museumButton.onClick.AddListener(OnClickMuseum);
        continueButton.onClick.AddListener(OnClickContinue);

        if (root) root.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    private void OnClickContinue()
    {
        if (root) root.SetActive(false);
        puzzleManager.ResumeTimer();
    }

    private void OnClickMuseum()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Museum", LoadSceneMode.Single);
    }

    private void PauseGame()
    {
        if (root) root.SetActive(true);
        isPaused = true;
        puzzleManager.PauseTimer();
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        if (root) root.SetActive(false);
        isPaused = false;
        puzzleManager.ResumeTimer();
    }
}
