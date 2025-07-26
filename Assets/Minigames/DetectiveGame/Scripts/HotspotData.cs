using UnityEngine;

[System.Serializable]
public class HotspotData
{
    public string name;
    public Vector2 position;
    public Sprite icon;
    public HotspotType type;
    public string linkedRoomName;
    public TextAsset dialogFile;
}