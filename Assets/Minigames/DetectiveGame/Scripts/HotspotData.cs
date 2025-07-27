using UnityEngine;

[System.Serializable]
public class HotspotData
{
    public string name;
    public Vector2 position;
    public float rotation = 0f;
    public Sprite icon;
    public float iconWidth = 40f;
    public float iconHeight = 40f;
    public HotspotType type;
    public string linkedRoomName;
    public TextAsset dialogFile;
    public Sprite inspectImage;
    public string inspectText;
    public bool removeAfterFound = false;
}