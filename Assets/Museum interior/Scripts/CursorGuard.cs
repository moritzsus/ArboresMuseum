using UnityEngine;

public class CursorGuard : MonoBehaviour
{
    void LateUpdate()
    {
        bool uiOpen = InfoPanelUI.Instance != null && InfoPanelUI.Instance.IsOpen;

        var wantLock = uiOpen ? CursorLockMode.None : CursorLockMode.Locked;
        var wantVis = uiOpen;

        if (Cursor.lockState != wantLock) Cursor.lockState = wantLock;
        if (Cursor.visible != wantVis) Cursor.visible = wantVis;
    }
}
