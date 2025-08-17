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

    private PictureInfo currentAim;
    private PictureInfo openedPlaque;

    private void Awake()
    {
        interactableMask = LayerMask.GetMask("Interactable");

        if (!playerCamera) playerCamera = GetComponentInChildren<Camera>();
        if (!infoUI) infoUI = InfoPanelUI.Instance;
        if (interactPrompt) interactPrompt.gameObject.SetActive(false);

        if (infoUI)
        {
            infoUI.OnOpened += () => { openedPlaque = currentAim; };
            infoUI.OnClosed += () => { openedPlaque = null; };
        }
    }

    private void Update()
    {
        if (!playerCamera) return;

        if (infoUI && infoUI.IsOpen && openedPlaque)
        {
            float dist = Vector3.Distance(playerCamera.transform.position, openedPlaque.FocusPosition);
            if (dist > openedPlaque.InteractRange + closeBuffer)
            {
                infoUI.Close();
            }

            if (interactPrompt) interactPrompt.gameObject.SetActive(false);
            return;
        }

        // Gaze-Raycast
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        currentAim = null;
        if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance, interactableMask, QueryTriggerInteraction.Ignore))
        {
            currentAim = hit.collider.GetComponentInParent<PictureInfo>()
                      ?? hit.collider.GetComponent<PictureInfo>();
        }

        bool canInteract = currentAim && IsInRange(currentAim);
        if (interactPrompt) interactPrompt.gameObject.SetActive(canInteract);

        if (canInteract && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            infoUI.Open(currentAim);
        }
    }

    private bool IsInRange(PictureInfo info)
    {
        float dist = Vector3.Distance(playerCamera.transform.position, info.FocusPosition);
        return dist <= info.InteractRange;
    }
}
