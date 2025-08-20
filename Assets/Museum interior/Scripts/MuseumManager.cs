using UnityEngine;

public class MuseumManager : MonoBehaviour
{
    [SerializeField] private GameObject[] minigameEntrances;
    [SerializeField] private DoorBarrier[] barriers;

    private void Start()
    {
        ApplyModeAndProgress();

        if (GameSettings.Instance != null)
            GameSettings.Instance.OnMinigameCompleted += _ => ApplyModeAndProgress();
    }

    private void OnDestroy()
    {
        if (GameSettings.Instance != null)
            GameSettings.Instance.OnMinigameCompleted -= _ => ApplyModeAndProgress();
    }

    public void ApplyModeAndProgress()
    {
        bool explore = GameSettings.Instance != null && GameSettings.Instance.Mode == GameMode.Explore;

        if (minigameEntrances != null)
            foreach (var go in minigameEntrances)
                if (go) go.SetActive(!explore);

        if (barriers != null)
            foreach (var b in barriers)
                if (b)
                {
                    if (explore)
                    {
                        if (b.gameObject.activeSelf) b.gameObject.SetActive(false);
                    }
                    else
                    {
                        if (!b.gameObject.activeSelf) b.gameObject.SetActive(true);
                        b.RefreshState();
                    }
                }
    }
}
