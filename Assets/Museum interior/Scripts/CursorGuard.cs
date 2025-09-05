using UnityEngine;

public class CursorGuard : MonoBehaviour
{
    public static CursorGuard Instance { get; private set; }

    private bool needsCursor;

    private void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void LateUpdate()
    {
        bool uiOpen = InfoPanelUI.Instance != null && InfoPanelUI.Instance.IsOpen;
        bool wantFreeCursor = uiOpen || needsCursor;

        var wantLock = wantFreeCursor ? CursorLockMode.None : CursorLockMode.Locked;
        var wantVis = wantFreeCursor;

        if (Cursor.lockState != wantLock) Cursor.lockState = wantLock;
        if (Cursor.visible != wantVis) Cursor.visible = wantVis;
    }

    public void SetNeedsCursor(bool needsCursor)
    {
        this.needsCursor = needsCursor;
    }
}
