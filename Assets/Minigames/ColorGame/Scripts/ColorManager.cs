using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance;

    public bool redUnlocked;
    public bool greenUnlocked;
    public bool blueUnlocked;

    public Volume volume;
    private ChannelMixer channelMixer;

    public UnityEvent OnColorUpdate = new UnityEvent();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (volume == null || volume.profile == null)
        {
            Debug.LogError("Volume oder Volume Profile fehlt!");
            return;
        }

        if (!volume.profile.TryGet(out channelMixer))
        {
            Debug.LogError("ChannelMixer fehlt im Volume Profile!");
            return;
        }

        SetGrayscale();
    }

    public void SetGrayscale()
    {
        float rLuma = 29.9f;
        float gLuma = 58.7f;
        float bLuma = 11.4f;

        channelMixer.redOutRedIn.value = rLuma;
        channelMixer.redOutGreenIn.value = gLuma;
        channelMixer.redOutBlueIn.value = bLuma;

        channelMixer.greenOutRedIn.value = rLuma;
        channelMixer.greenOutGreenIn.value = gLuma;
        channelMixer.greenOutBlueIn.value = bLuma;

        channelMixer.blueOutRedIn.value = rLuma;
        channelMixer.blueOutGreenIn.value = gLuma;
        channelMixer.blueOutBlueIn.value = bLuma;
    }

    public void UnlockColor(Color color)
    {
        if (channelMixer == null)
            return;

        if (color == Color.red)
        {
            channelMixer.redOutRedIn.value = 100f;
            redUnlocked = true;
        }
        else if (color == Color.green)
        {
            channelMixer.greenOutGreenIn.value = 100f;
            greenUnlocked = true;
        }
        else if (color == Color.blue)
        {
            channelMixer.blueOutBlueIn.value = 100f;
            blueUnlocked = true;
        }

        OnColorUpdate.Invoke();
    }

    public Color GetCurrentColor()
    {
        return new Color(
            redUnlocked ? 1f : 0f,
            greenUnlocked ? 1f : 0f,
            blueUnlocked ? 1f : 0f
        );
    }

    public bool AllColorsUnlocked()
    {
        return redUnlocked && greenUnlocked && blueUnlocked;
    }
}
