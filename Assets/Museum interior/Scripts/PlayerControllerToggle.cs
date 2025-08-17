using UnityEngine;

public class PlayerControllerToggle : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] toDisable;

    private void OnEnable()
    {
        if (InfoPanelUI.Instance != null)
        {
            InfoPanelUI.Instance.OnOpened += HandleOpen;
            InfoPanelUI.Instance.OnClosed += HandleClose;
        }
    }
    private void OnDisable()
    {
        if (InfoPanelUI.Instance != null)
        {
            InfoPanelUI.Instance.OnOpened -= HandleOpen;
            InfoPanelUI.Instance.OnClosed -= HandleClose;
        }
    }

    private void HandleOpen()
    {
        foreach (var comp in toDisable) if (comp) comp.enabled = false;
    }
    private void HandleClose()
    {
        foreach (var comp in toDisable) if (comp) comp.enabled = true;
    }
}
