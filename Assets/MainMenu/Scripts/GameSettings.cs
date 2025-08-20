using System;
using UnityEngine;

public enum GameMode
{
    Play,
    Explore
}

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    public GameMode Mode { get; private set; } = GameMode.Play;
    public string PlayerName { get; private set; } = "";
    public int CharacterIndex { get; private set; } = 0;

    private readonly bool[] minigameCompleted = new bool[4];
    public Action<int> OnMinigameCompleted;

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
        OnMinigameCompleted?.Invoke(index);
    }
}
