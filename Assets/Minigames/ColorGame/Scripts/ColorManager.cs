using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance;

    public bool redUnlocked;
    public bool greenUnlocked;
    public bool blueUnlocked;

    public GameObject redShard;
    public GameObject greenShard;
    public GameObject blueShard;

    public Image redUiFill;
    public Image greenUiFill;
    public Image blueUiFill;

    public float flashDuration = 1.45f;
    public float overshootFactor = 1.9f;

    public Volume volume;
    private ChannelMixer channelMixer;

    public UnityEvent OnColorUpdate = new UnityEvent();

    private const float LUMA_R = 29.9f;
    private const float LUMA_G = 58.7f;
    private const float LUMA_B = 11.4f;

    private List<(float, float)> redShardPositions = new List<(float, float)>
    {
        (76f, 73f),
        (31f, 51f)
    };
    private List<(float, float)> greenShardPositions = new List<(float, float)>
    {
        (-108f, -23f),
        (-44f, -24f),
        (-12f, -25f),
        (-8f, -27f),
        (12f, -14f),
        (27f, -5f),
        (51f, -26f),
        (131f, -8.5f),
        (137f, -27f)
    };
    private List<(float, float)> blueShardPositions = new List<(float, float)>
    {
        (85f, 21f),
        (153f, 1f)
    };

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

        EnsureOverridesEnabled();
        ApplyMixerFromFlags();
        AssignRandomShardPositions();
    }

    public void SetGrayscale()
    {
        redUnlocked = greenUnlocked = blueUnlocked = false;
        ApplyMixerFromFlags();
    }

    public void UnlockColor(Color color)
    {
        if (channelMixer == null)
            return;

        if (color == Color.red)
        {
            redUiFill.color = Color.red;
            redUnlocked = true;
        }
        else if (color == Color.green)
        {
            greenUiFill.color = Color.green;
            greenUnlocked = true;
        }
        else if (color == Color.blue)
        {
            blueUiFill.color = Color.blue;
            blueUnlocked = true;
        }

        ApplyMixerFromFlags();

        float baseTarget = 100f;
        float boosted = baseTarget * overshootFactor;

        SetDiagonal(color, boosted);

        StartCoroutine(FadeDiagonalTo(color, baseTarget, flashDuration));

        OnColorUpdate.Invoke(); // TODO needed?
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

    private void AssignRandomShardPositions()
    {
        if (redShard != null && redShardPositions.Count > 0)
        {
            var randomRedPos = redShardPositions[Random.Range(0, redShardPositions.Count)];
            redShard.transform.position = new Vector3(randomRedPos.Item1, randomRedPos.Item2, redShard.transform.position.z);
        }

        if (greenShard != null && greenShardPositions.Count > 0)
        {
            var randomGreenPos = greenShardPositions[Random.Range(0, greenShardPositions.Count)];
            greenShard.transform.position = new Vector3(randomGreenPos.Item1, randomGreenPos.Item2, greenShard.transform.position.z);
        }

        if (blueShard != null && blueShardPositions.Count > 0)
        {
            var randomBluePos = blueShardPositions[Random.Range(0, blueShardPositions.Count)];
            blueShard.transform.position = new Vector3(randomBluePos.Item1, randomBluePos.Item2, blueShard.transform.position.z);
        }
    }

    private void SetDiagonal(Color color, float value)
    {
        if (color == Color.red) channelMixer.redOutRedIn.value = value;
        else if (color == Color.green) channelMixer.greenOutGreenIn.value = value;
        else if (color == Color.blue) channelMixer.blueOutBlueIn.value = value;
    }

    private IEnumerator FadeDiagonalTo(Color color, float target, float duration)
    {
        float start = GetDiagonal(color);
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / duration);
            float k = 1f - Mathf.Pow(1f - p, 3f);
            float val = Mathf.Lerp(start, target, k);
            SetDiagonal(color, val);
            yield return null;
        }

        SetDiagonal(color, target);
        OnColorUpdate.Invoke();
    }

    private float GetDiagonal(Color color)
    {
        if (color == Color.red) return channelMixer.redOutRedIn.value;
        else if (color == Color.green) return channelMixer.greenOutGreenIn.value;
        else return channelMixer.blueOutBlueIn.value;
    }

    private void EnsureOverridesEnabled()
    {
        channelMixer.redOutRedIn.overrideState = true;
        channelMixer.redOutGreenIn.overrideState = true;
        channelMixer.redOutBlueIn.overrideState = true;

        channelMixer.greenOutRedIn.overrideState = true;
        channelMixer.greenOutGreenIn.overrideState = true;
        channelMixer.greenOutBlueIn.overrideState = true;

        channelMixer.blueOutRedIn.overrideState = true;
        channelMixer.blueOutGreenIn.overrideState = true;
        channelMixer.blueOutBlueIn.overrideState = true;
    }

    private void ApplyMixerFromFlags()
    {
        // R-Ausgangszeile
        if (redUnlocked) SetRowRed(100f, 0f, 0f);     // Identität: R'=R
        else SetRowRed(LUMA_R, LUMA_G, LUMA_B); // Graustufe: R'=Y

        // G-Ausgangszeile
        if (greenUnlocked) SetRowGreen(0f, 100f, 0f); // G'=G
        else SetRowGreen(LUMA_R, LUMA_G, LUMA_B); // G'=Y

        // B-Ausgangszeile
        if (blueUnlocked) SetRowBlue(0f, 0f, 100f);   // B'=B
        else SetRowBlue(LUMA_R, LUMA_G, LUMA_B);   // B'=Y

        OnColorUpdate.Invoke();
    }

    private void SetRowRed(float r, float g, float b)
    {
        channelMixer.redOutRedIn.value = r;
        channelMixer.redOutGreenIn.value = g;
        channelMixer.redOutBlueIn.value = b;
    }

    private void SetRowGreen(float r, float g, float b)
    {
        channelMixer.greenOutRedIn.value = r;
        channelMixer.greenOutGreenIn.value = g;
        channelMixer.greenOutBlueIn.value = b;
    }

    private void SetRowBlue(float r, float g, float b)
    {
        channelMixer.blueOutRedIn.value = r;
        channelMixer.blueOutGreenIn.value = g;
        channelMixer.blueOutBlueIn.value = b;
    }
}
