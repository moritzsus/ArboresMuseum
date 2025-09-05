using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DetectiveEndUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject root;
    [SerializeField] private Button museumButton;
    [SerializeField] private Button replayButton;

    private void Start()
    {
        museumButton.onClick.AddListener(OnClickMuseum);
        replayButton.onClick.AddListener(OnClickReplay);
    }

    private void OnClickMuseum()
    {
        SceneManager.LoadScene("Museum", LoadSceneMode.Single);
    }

    private void OnClickReplay()
    {
        SceneManager.LoadScene("DetectiveGame", LoadSceneMode.Single);
    }
}
