using UnityEngine;

public class MuseumManager : MonoBehaviour
{
    [SerializeField] private GameObject[] minigameEntrances;
    [SerializeField] private DoorBarrier[] barriers;

    private void Start()
    {
        Time.timeScale = 1f;
        bool explore = GameSettings.Instance != null && GameSettings.Instance.Mode == GameMode.Explore;

        foreach (var mg in minigameEntrances)
        {
            mg.SetActive(!explore);
        }

        for (int i = 0; i < barriers.Length; i++)
        {
            barriers[i].gameObject.SetActive(!explore && !GameSettings.Instance.IsMinigameCompleted(i));
        }

        // Set last barrier inactive after game 3 done => no more barriers needed
        if (!explore && GameSettings.Instance.IsMinigameCompleted(2))
            barriers[3].gameObject.SetActive(false);
    }
}
