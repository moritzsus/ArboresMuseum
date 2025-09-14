using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 7.0f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Look")]
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private float mouseSensitivity = 4.0f;
    [SerializeField] private float minPitch = -89f;
    [SerializeField] private float maxPitch = 89f;

    [Header("Cursor")]
    [SerializeField] private bool lockCursor = true;

    private CharacterController controller;
    private float pitch;
    private float verticalVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (!cameraRoot)
        {
            var cam = GetComponentInChildren<Camera>();
            if (cam) cameraRoot = cam.transform;
        }
    }

    void Start()
    {
        if (GameSettings.Instance != null)
        {
            mouseSensitivity = GameSettings.Instance.MouseSensitivity;
        }

        ApplyCursorLock(lockCursor);
    }

    void ApplyCursorLock(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
    }

    void HandleLook()
    {
        if (!cameraRoot) return;
        Vector2 delta = Mouse.current != null ? Mouse.current.delta.ReadValue() : Vector2.zero;
        delta *= mouseSensitivity * Time.deltaTime;

        // Yaw
        transform.Rotate(Vector3.up * delta.x);

        // Pitch
        pitch = Mathf.Clamp(pitch - delta.y, minPitch, maxPitch);
        var e = cameraRoot.localEulerAngles;
        e.x = pitch;
        cameraRoot.localEulerAngles = e;
    }

    void HandleMovement()
    {
        Vector2 moveInput = Vector2.zero;
        if (Keyboard.current.wKey.isPressed) moveInput.y += 1;
        if (Keyboard.current.sKey.isPressed) moveInput.y -= 1;
        if (Keyboard.current.aKey.isPressed) moveInput.x -= 1;
        if (Keyboard.current.dKey.isPressed) moveInput.x += 1;
        if (moveInput.sqrMagnitude > 1f) moveInput.Normalize();

        Vector3 move = (transform.right * moveInput.x) + (transform.forward * moveInput.y);

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = (move * speed) + (Vector3.up * verticalVelocity);
        controller.Move(velocity * Time.deltaTime);
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        mouseSensitivity = sensitivity;
    }
}
