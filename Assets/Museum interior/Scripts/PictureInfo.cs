using UnityEngine;

public class PictureInfo : MonoBehaviour
{
    [SerializeField]
    private string title;
    [TextArea(3, 10)]
    [SerializeField]
    private string info;
    [SerializeField]
    private float interactRange = 2.5f;
    [SerializeField]
    private Transform focusPoint;

    public string Title => title;
    public string Info => info;
    public float InteractRange => interactRange;
    public Vector3 FocusPosition => focusPoint ? focusPoint.position : transform.position;
}
