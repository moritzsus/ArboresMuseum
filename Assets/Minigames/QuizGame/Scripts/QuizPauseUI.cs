using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuizPauseUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject root;
    [SerializeField] private Button museumButton;
    [SerializeField] private Button continueButton;

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
    }

    private void OnClickMuseum()
    {
        SceneManager.LoadScene("Museum", LoadSceneMode.Single);
    }

    private void PauseGame()
    {
        if (root) root.SetActive(true);
        isPaused = true;
    }

    private void ResumeGame()
    {
        if (root) root.SetActive(false);
        isPaused = false;
    }
}
