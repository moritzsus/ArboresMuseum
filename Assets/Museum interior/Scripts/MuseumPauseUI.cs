using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MuseumPauseUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject root;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button continueButton;

    [Header("Sensitivity Settings")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TextMeshProUGUI sensitivityValueText;

    private bool isPaused = false;
    private FirstPersonController playerController;

    //private void Start()
    //{
    //    mainMenuButton.onClick.AddListener(OnClickMainMenu);
    //    continueButton.onClick.AddListener(OnClickContinue);

    //    if (root) root.SetActive(false);
    //}

    private void Start()
    {
        mainMenuButton.onClick.AddListener(OnClickMainMenu);
        continueButton.onClick.AddListener(OnClickContinue);

        if (sensitivitySlider != null)
        {
            sensitivitySlider.minValue = 1.0f;
            sensitivitySlider.maxValue = 10.0f;

            sensitivitySlider.value = GameSettings.Instance.MouseSensitivity;

            sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);

            UpdateSensitivityText(sensitivitySlider.value);
        }

        var player = GameObject.FindWithTag("Player");
        if (player)
            playerController = player.GetComponent<FirstPersonController>();

        if (root) root.SetActive(false);
    }

    private void OnSensitivityChanged(float newValue)
    {
        GameSettings.Instance.MouseSensitivity = newValue;

        UpdateSensitivityText(newValue);

        if (playerController != null)
            playerController.SetMouseSensitivity(newValue);
    }

    private void UpdateSensitivityText(float value)
    {
        if (sensitivityValueText != null)
            sensitivityValueText.text = value.ToString("F1");
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
}
