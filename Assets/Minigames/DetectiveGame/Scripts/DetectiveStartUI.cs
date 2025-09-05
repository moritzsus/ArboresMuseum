using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DetectiveStartUI : MonoBehaviour
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
        if (startButton) startButton.onClick.RemoveListener(OnClickStart);
        if (root) root.SetActive(false);
    }

    private void OnClickBack()
    {
        SceneManager.LoadScene("Museum", LoadSceneMode.Single);
    }
}
