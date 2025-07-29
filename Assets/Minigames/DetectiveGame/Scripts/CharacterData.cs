using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public string name;
    public bool isClue;
    public Vector2 position;
    public Vector2 size = new Vector2(100, 200);
    public Sprite sprite;
    public Sprite characterDialogIcon;
    public TextAsset dialogueFile;
    public TextAsset alternateDialogueFile;
}
