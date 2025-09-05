using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ColorStartUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject root;
    [SerializeField] private Button startButton;
    [SerializeField] private Button backButton;

    private void Start()
    {
        startButton.onClick.AddListener(OnClickStart);
        backButton.onClick.AddListener(OnClickBack);
    }

    private void OnClickStart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (startButton) startButton.onClick.RemoveListener(OnClickStart);
        if (root) root.SetActive(false);

        ColorManager.Instance.StartTimer();
        Time.timeScale = 1f;
    }

    private void OnClickBack()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Museum", LoadSceneMode.Single);
    }
}
