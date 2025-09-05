using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ColorPauseUI : MonoBehaviour
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
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ColorManager.Instance.ResumeTimer();
    }

    private void OnClickMuseum()
    {
        SceneManager.LoadScene("Museum", LoadSceneMode.Single);
    }

    private void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (root) root.SetActive(true);
        isPaused = true;

        ColorManager.Instance.PauseTimer();
    }

    private void ResumeGame()
    {
        if (root) root.SetActive(false);
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ColorManager.Instance.ResumeTimer();
    }
}
