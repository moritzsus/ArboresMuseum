using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzlePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Outline glowEffect;

    private Vector2 startPos;
    private Vector2 targetPos;
    private Vector2 dragOffset;
    private int correctIndex;

    private bool isSnapping = false;

    public void Init(Vector2 targetPosition, int correctSpriteIndex)
    {
        targetPos = targetPosition;
        correctIndex = correctSpriteIndex;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        glowEffect = GetComponent<Outline>();
        if (glowEffect == null)
        {
            glowEffect = gameObject.AddComponent<Outline>();
        }
        glowEffect.effectColor = new Color(1f, 1f, 0f, 0f);
        glowEffect.enabled = true;

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        rectTransform.SetAsLastSibling();

        RectTransform parentRect = rectTransform.parent as RectTransform;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint))
        {
            dragOffset = rectTransform.anchoredPosition - localPoint;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isSnapping) return;

        RectTransform parentRect = rectTransform.parent as RectTransform;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint))
        {
            rectTransform.anchoredPosition = localPoint + dragOffset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (Vector2.Distance(rectTransform.anchoredPosition, targetPos) < 30f)
        {
            StartCoroutine(SnapToPosition());
        }
    }

    private IEnumerator SnapToPosition()
    {
        isSnapping = true;
        float duration = 0.25f;
        float elapsed = 0f;

        Vector2 start = rectTransform.anchoredPosition;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            rectTransform.anchoredPosition = Vector2.Lerp(start, targetPos, elapsed / duration);
            yield return null;
        }

        rectTransform.anchoredPosition = targetPos;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        FindFirstObjectByType<PuzzleManager>().NotifyPieceCorrect();

        StartCoroutine(PlayGlowEffect());
    }

    private IEnumerator PlayGlowEffect()
    {
        if (glowEffect == null)
            yield break;

        float duration = 0.5f;
        float elapsed = 0f;
        float startAlpha = 0.8f;
        float endAlpha = 0f;

        float startDistance = 5f;
        float endDistance = 15f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            float alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            float distance = Mathf.Lerp(startDistance, endDistance, t);

            glowEffect.effectColor = new Color(1f, 1f, 0f, alpha);
            glowEffect.effectDistance = new Vector2(distance, distance);

            yield return null;
        }

        glowEffect.effectColor = new Color(1f, 1f, 0f, 0f);
        glowEffect.effectDistance = new Vector2(startDistance, startDistance);
    }


    private IEnumerator PlayBounceEffect()
    {
        Vector3 originalScale = rectTransform.localScale;
        Vector3 targetScale = originalScale * 1.15f;
        float duration = 0.15f;
        float elapsed = 0f;

        // Scale up
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            rectTransform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }

        // Scale down
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            rectTransform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }

        rectTransform.localScale = originalScale;
        isSnapping = false;
    }

}
