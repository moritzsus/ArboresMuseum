using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameMachine : MonoBehaviour
{
    [Header("Scene to load")]
    [SerializeField] private string sceneName;

    [Header("Interaction")]
    [SerializeField] private float interactRange = 2.5f;
    [SerializeField] private Transform focusPoint;
    [SerializeField] private string promptText = "E – Spielen";

    public float InteractRange => interactRange;
    public Vector3 FocusPosition => focusPoint ? focusPoint.position : transform.position;
    public string Prompt => promptText;

    public void Play()
    {
        var player = GameObject.FindWithTag("Player");
        if (player && GameSettings.Instance != null)
            GameSettings.Instance.SetMuseumReturn(player.transform);

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
