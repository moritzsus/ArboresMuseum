using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private InfoPanelUI infoUI;
    [SerializeField] private TMP_Text interactPrompt;

    [Header("Interaction")]
    [SerializeField] private float maxRayDistance = 3.0f;
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private float closeBuffer = 0.5f;

    private PictureInfo currentPlaque;
    private PictureInfo openedPlaque;
    private MinigameMachine currentMachine;
    private DoorBarrier currentBarrier;

    private void Awake()
    {
        if (interactableMask == 0) interactableMask = LayerMask.GetMask("Interactable");

        if (!playerCamera) playerCamera = GetComponentInChildren<Camera>();
        if (!infoUI) infoUI = InfoPanelUI.Instance;
        if (interactPrompt) interactPrompt.gameObject.SetActive(false);

        if (infoUI)
        {
            infoUI.OnOpened += () => { openedPlaque = currentPlaque; };
            infoUI.OnClosed += () => { openedPlaque = null; };
        }
    }

    private void Update()
    {
        if (!playerCamera) return;

        if (infoUI && infoUI.IsOpen)
        {
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                infoUI.Close();
                if (interactPrompt) interactPrompt.gameObject.SetActive(false);
                return;
            }

            // auto close on walk away
            if (openedPlaque)
            {
                float dist = Vector3.Distance(playerCamera.transform.position, openedPlaque.FocusPosition);
                if (dist > openedPlaque.InteractRange + closeBuffer)
                    infoUI.Close();
            }

            if (interactPrompt) interactPrompt.gameObject.SetActive(false);
            return;
        }

        // Gaze-Raycast
        currentPlaque = null;
        currentMachine = null;
        currentBarrier = null;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, interactableMask, QueryTriggerInteraction.Ignore))
        {
            currentMachine = hit.collider.GetComponentInParent<MinigameMachine>()
                         ?? hit.collider.GetComponent<MinigameMachine>();

            if (!currentMachine)
            {
                currentBarrier = hit.collider.GetComponentInParent<DoorBarrier>() ?? hit.collider.GetComponent<DoorBarrier>();
                if (!currentBarrier)
                {
                    currentPlaque = hit.collider.GetComponentInParent<PictureInfo>() ?? hit.collider.GetComponent<PictureInfo>();
                }
            }
        }

        bool show = false;

        if (currentMachine)
        {
            show = InRange(currentMachine.FocusPosition, currentMachine.InteractRange);
            if (interactPrompt) interactPrompt.text = currentMachine.Prompt;
        }
        else if (currentBarrier)
        {
            show = InRange(currentBarrier.FocusPosition, currentBarrier.InteractRange);
            if (interactPrompt) interactPrompt.text = currentBarrier.LockedPrompt;
        }
        else if (currentPlaque)
        {
            show = InRange(currentPlaque.FocusPosition, currentPlaque.InteractRange);
            if (interactPrompt) interactPrompt.text = "E – Lesen";
        }

        if (interactPrompt) interactPrompt.gameObject.SetActive(show);

        if (show && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (currentMachine) currentMachine.Play();
            else if (currentPlaque) infoUI.Open(currentPlaque);
        }
    }

    private bool InRange(Vector3 focusPos, float range)
    {
        return Vector3.Distance(playerCamera.transform.position, focusPos) <= range;
    }
}
