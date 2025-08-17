using UnityEngine;

public class CrosshairUI : MonoBehaviour
{
    [SerializeField] private GameObject crosshairRoot;

    void Awake()
    {
        if (crosshairRoot) crosshairRoot.SetActive(false);
    }

    void OnEnable()
    {
        if (InfoPanelUI.Instance != null)
        {
            InfoPanelUI.Instance.OnOpened += Hide;
            InfoPanelUI.Instance.OnClosed += ShowIfGameplay;
        }
    }
    void OnDisable()
    {
        if (InfoPanelUI.Instance != null)
        {
            InfoPanelUI.Instance.OnOpened -= Hide;
            InfoPanelUI.Instance.OnClosed -= ShowIfGameplay;
        }
    }

    void Update()
    {
        if (!InfoPanelUI.Instance || !InfoPanelUI.Instance.IsOpen)
        {
            bool gameplay = Cursor.lockState == CursorLockMode.Locked && !Cursor.visible;
            if (crosshairRoot && crosshairRoot.activeSelf != gameplay)
                crosshairRoot.SetActive(gameplay);
        }
    }

    private void ShowIfGameplay()
    {
        if (crosshairRoot)
            crosshairRoot.SetActive(Cursor.lockState == CursorLockMode.Locked && !Cursor.visible);
    }

    private void Hide()
    {
        if (crosshairRoot) crosshairRoot.SetActive(false);
    }
}
