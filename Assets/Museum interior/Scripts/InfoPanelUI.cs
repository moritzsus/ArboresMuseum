using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelUI : MonoBehaviour
{
    public static InfoPanelUI Instance { get; private set; }

    [Header("Refs")]
    [SerializeField] private CanvasGroup panelGroup;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private Button closeButton;

    [Header("Fade")]
    [SerializeField] private float fadeDuration = 0.12f;

    public bool IsOpen { get; private set; }

    public event System.Action OnOpened;
    public event System.Action OnClosed;

    private void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        SetVisible(false, instant: true);
        if (closeButton) closeButton.onClick.AddListener(Close);
    }

    public void Open(PictureInfo info)
    {
        if (!info) return;
        titleText.text = info.Title;
        infoText.text = info.Info;
        StopAllCoroutines();
        StartCoroutine(Fade(true));
    }

    public void Close()
    {
        if (!IsOpen) return;
        StopAllCoroutines();
        StartCoroutine(Fade(false));
    }

    private IEnumerator Fade(bool show)
    {
        float from = panelGroup.alpha;
        float to = show ? 1f : 0f;
        float t = 0f;

        if (show)
        {
            panelGroup.interactable = true;
            panelGroup.blocksRaycasts = true;
        }
        else
        {
            IsOpen = false;
            panelGroup.interactable = false;
            panelGroup.blocksRaycasts = false;
            OnClosed?.Invoke();
        }

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            panelGroup.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }
        panelGroup.alpha = to;

        if (show)
        {
            IsOpen = true;
            OnOpened?.Invoke();
        }
    }

    private void SetVisible(bool visible, bool instant = false)
    {
        panelGroup.alpha = visible ? 1f : 0f;
        panelGroup.interactable = visible;
        panelGroup.blocksRaycasts = visible;
        IsOpen = visible;
        if (instant) StopAllCoroutines();
    }
}
