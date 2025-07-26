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
    public int? nextLineIndex;
}

[System.Serializable]
public class DialogueChoice
{
    public string text;
    public int nextLineIndex;
}
