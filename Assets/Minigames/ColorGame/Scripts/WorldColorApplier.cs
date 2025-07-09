using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WorldColorApplier : MonoBehaviour
{
    public Volume volume;
    private ColorAdjustments colorAdjustments;

    void Start()
    {
        if (volume.profile.TryGet(out colorAdjustments))
        {
            ColorManager.Instance.OnColorUpdate.AddListener(UpdateColors);
            UpdateColors();
        }
    }

    private void UpdateColors()
    {
        Debug.Log("Update");
        colorAdjustments.colorFilter.value = ColorManager.Instance.GetCurrentColor();
    }
}
