using UnityEngine;

public class MuseumSpawnSetter : MonoBehaviour
{
    [SerializeField] private Transform defaultSpawn;

    private void Start()
    {
        var player = GameObject.FindWithTag("Player");
        if (!player) return;

        if (GameSettings.Instance != null &&
            GameSettings.Instance.TryConsumeMuseumReturn(out var pos, out var rot))
        {
            var cc = player.GetComponent<CharacterController>();
            if (cc) cc.enabled = false;
            player.transform.SetPositionAndRotation(pos, rot);
            if (cc) cc.enabled = true;
        }
        else if (defaultSpawn)
        {
            var cc = player.GetComponent<CharacterController>();
            if (cc) cc.enabled = false;
            player.transform.SetPositionAndRotation(defaultSpawn.position, defaultSpawn.rotation);
            if (cc) cc.enabled = true;
        }
    }
}
