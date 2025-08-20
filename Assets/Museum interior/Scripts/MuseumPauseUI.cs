using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MuseumPauseUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject root;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button continueButton;

    private bool isPaused = false;

    private void Start()
    {
        mainMenuButton.onClick.AddListener(OnClickMainMenu);
        continueButton.onClick.AddListener(OnClickContinue);

        if (root) root.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                OnClickContinue();
            else
                PauseGame();
        }
    }

    private void OnClickContinue()
    {
        if (root) root.SetActive(false);
        Time.timeScale = 1f;
        CursorGuard.Instance.SetNeedsCursor(false);
        isPaused = false;
    }

    private void OnClickMainMenu()
    {
        GameSettings.Instance.ResetGameState();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    private void PauseGame()
    {
        if (root) root.SetActive(true);
        Time.timeScale = 0f;
        CursorGuard.Instance.SetNeedsCursor(true);
        isPaused = true;
    }

    //private void ResumeGame()
    //{
    //    if (root) root.SetActive(false);
    //    Time.timeScale = 1f;
    //    CursorGuard.Instance.SetNeedsCursor(false);
    //    isPaused = false;
    //}
}
