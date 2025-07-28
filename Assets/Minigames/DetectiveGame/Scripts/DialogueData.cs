[System.Serializable]
public class DialogueData
{
    public DialogueLine[] lines;
}

[System.Serializable]
public class DialogueLine
{
    public string speaker;
    public string text;
    public DialogueChoice[] choices;
    public int nextLineIndex = -1; // -1 => go to next index
}

[System.Serializable]
public class DialogueChoice
{
    public string text;
    public int nextLineIndex;
}

[System.Serializable]
public class OfficerDialogueWrapper
{
    public DialogueData policeInsufficientClues;
    public DialogueData policeEnoughClues;
}
