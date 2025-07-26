using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomData", menuName = "Scriptable /Detective/RoomData")]
public class RoomData : ScriptableObject
{
    public Sprite backgroundImage;
    public List<HotspotData> hotspots;
    public List<CharacterData> characters;
}
