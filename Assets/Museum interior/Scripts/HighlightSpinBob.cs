using UnityEngine;

public class HighlightSpinBob : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private bool rotateAroundWorldY = true;

    [Header("Bobbing (Up/Down)")]
    [SerializeField] private float bobAmplitude = 0.06f;
    [SerializeField] private float bobFrequency = 0.4f;
    [SerializeField] private bool useLocalPosition = false;

    private Vector3 startPos;

    private void OnEnable()
    {
        startPos = useLocalPosition ? transform.localPosition : transform.position;
    }

    private void OnDisable()
    {
        if (useLocalPosition) transform.localPosition = startPos;
        else transform.position = startPos;
    }

    private void Update()
    {
        if (!Application.isPlaying) return;

        // Optional pausieren, wenn deine Info-UI offen ist
        //if (pauseWhenInfoOpen && InfoPanelUI.Instance != null && InfoPanelUI.Instance.IsOpen)
        //    return;

        float dt = Time.deltaTime;

        // 1) rotation
        float angle = rotationSpeed * dt;
        transform.Rotate(Vector3.up, angle, rotateAroundWorldY ? Space.World : Space.Self);

        // up and down
        float yOffset = Mathf.Sin(Time.time * (Mathf.PI * 2f) * bobFrequency) * bobAmplitude;

        if (useLocalPosition)
            transform.localPosition = new Vector3(startPos.x, startPos.y + yOffset, startPos.z);
        else
            transform.position = new Vector3(startPos.x, startPos.y + yOffset, startPos.z);
    }
}
