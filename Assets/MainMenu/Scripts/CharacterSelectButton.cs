using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour
{
    [SerializeField] private Outline outline;
    [SerializeField] private Image portrait;
    [SerializeField] private StartMenuController menu;
    [SerializeField] private int index;

    [Range(0.3f, 1f)][SerializeField] private float selectedBrightness = 1f;
    [Range(0.3f, 1f)][SerializeField] private float dimmedBrightness = 0.55f;

    public int Index => index;

    private void Reset()
    {
        if (!outline) outline = GetComponentInChildren<Outline>(true);
        if (!portrait) portrait = GetComponentInChildren<Image>(true);
        if (!menu) menu = GetComponentInParent<StartMenuController>();
    }

    private void Awake()
    {
        if (!outline) outline = GetComponentInChildren<Outline>(true);
        if (!portrait) portrait = GetComponentInChildren<Image>(true);
        if (!menu) menu = GetComponentInParent<StartMenuController>();
    }

    public void OnClick()
    {
        if (menu) menu.SelectCharacter(index);
    }

    public void SetSelected(bool selected)
    {
        if (outline) outline.enabled = selected;
        if (portrait) portrait.color = selected
            ? new Color(selectedBrightness, selectedBrightness, selectedBrightness, 1f)
            : new Color(dimmedBrightness, dimmedBrightness, dimmedBrightness, 1f);
    }
}
