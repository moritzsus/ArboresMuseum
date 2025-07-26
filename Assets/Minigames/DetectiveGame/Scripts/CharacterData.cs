using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public string name;
    public Vector2 position;
    public Vector2 size = new Vector2(100, 200);
    public Sprite sprite;
    public TextAsset dialogueFile;
}
