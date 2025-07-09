using UnityEngine;

public class ColorPickup : MonoBehaviour
{
    public Color colorToUnlock = Color.white;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ColorManager.Instance.UnlockColor(colorToUnlock);
            Destroy(gameObject);
        }
    }
}
