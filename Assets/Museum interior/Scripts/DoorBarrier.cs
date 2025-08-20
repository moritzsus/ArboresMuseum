using UnityEngine;

public class DoorBarrier : MonoBehaviour
{
    [SerializeField] private int requiredMinigameIndex = 0;

    [Header("Visual & Collider")]
    [SerializeField] private GameObject visualRoot;
    [SerializeField] private Collider barrierCollider;

    [Header("Interaction")]
    [SerializeField] private Transform focusPoint;
    [SerializeField] private float interactRange = 2.5f;
    [TextArea]
    [SerializeField]
    private string lockedPrompt =
        "Spiele zuerst das Minispiel, um in den nächsten Raum zu gelangen.";

    private int defaultLayer;

    private void Awake()
    {
        if (!barrierCollider) barrierCollider = GetComponent<Collider>();
        defaultLayer = gameObject.layer;
    }

    private void OnEnable()
    {
        if (GameSettings.Instance != null)
            GameSettings.Instance.OnMinigameCompleted += HandleCompleted;
        RefreshState();
    }

    private void OnDisable()
    {
        if (GameSettings.Instance != null)
            GameSettings.Instance.OnMinigameCompleted -= HandleCompleted;
    }

    private void HandleCompleted(int idx)
    {
        if (idx == requiredMinigameIndex) RefreshState();
    }

    public void RefreshState()
    {
        bool playMode = GameSettings.Instance == null || GameSettings.Instance.Mode == GameMode.Play;
        bool done = GameSettings.Instance != null && GameSettings.Instance.IsMinigameCompleted(requiredMinigameIndex);

        bool locked = playMode && !done;

        if (visualRoot) visualRoot.SetActive(locked);
        if (barrierCollider) barrierCollider.enabled = locked;

        gameObject.layer = locked ? LayerMask.NameToLayer("Interactable") : defaultLayer;
    }

    public Vector3 FocusPosition => focusPoint ? focusPoint.position : transform.position;
    public float InteractRange => interactRange;
    public bool IsLocked
    {
        get
        {
            bool playMode = GameSettings.Instance == null || GameSettings.Instance.Mode == GameMode.Play;
            bool done = GameSettings.Instance != null && GameSettings.Instance.IsMinigameCompleted(requiredMinigameIndex);
            return playMode && !done;
        }
    }
    public string LockedPrompt => lockedPrompt;
}
