using UnityEngine;

public enum GameMode
{
    Play,
    Explore
}

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    // TODO Attribute needed?
    [System.NonSerialized] private bool hasMuseumReturn;
    [System.NonSerialized] private Vector3 museumReturnPos;
    [System.NonSerialized] private Quaternion museumReturnRot;

    public GameMode Mode { get; private set; } = GameMode.Play;
    public string PlayerName { get; private set; } = "";
    public int CharacterIndex { get; private set; } = 0;
    public bool MuseumIntroSeen { get; set; } = false;

    private readonly bool[] minigameCompleted = new bool[4];

    private void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Apply(GameMode mode, string name, int charIndex)
    {
        Mode = mode;
        PlayerName = name;
        CharacterIndex = Mathf.Clamp(charIndex, 0, 3);
    }

    public bool IsMinigameCompleted(int index)
        => index >= 0 && index < minigameCompleted.Length && minigameCompleted[index];

    public void MarkMinigameCompleted(int index)
    {
        if (index < 0 || index >= minigameCompleted.Length) return;
        if (minigameCompleted[index]) return;
        minigameCompleted[index] = true;
    }

    public void SetMuseumReturn(Transform t)
    {
        hasMuseumReturn = true;
        museumReturnPos = t.position;

        var yaw = t.rotation.eulerAngles.y;
        museumReturnRot = Quaternion.Euler(0f, yaw, 0f);
    }

    public bool TryConsumeMuseumReturn(out Vector3 pos, out Quaternion rot)
    {
        if (hasMuseumReturn)
        {
            pos = museumReturnPos;
            rot = museumReturnRot;
            hasMuseumReturn = false;
            return true;
        }
        pos = default;
        rot = default;

        return false;
    }

    public void ResetGameState()
    {
        Mode = GameMode.Play;
        PlayerName = "";
        CharacterIndex = 0;
        MuseumIntroSeen = false;

        for (int i = 0; i < minigameCompleted.Length; i++)
        {
            minigameCompleted[i] = false;
        }

        hasMuseumReturn = false;
        museumReturnPos = new Vector3(31.629f, 1.1f, 13.2f);
        museumReturnRot = Quaternion.identity;
    }
}
