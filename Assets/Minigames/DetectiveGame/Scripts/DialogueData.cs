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
}

[System.Serializable]
public class DialogueChoice
{
    public string text;
    public int nextLineIndex;
}
