using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    private CharacterController controller;
    private Vector2 moveInput;

    [Header("Camera")]
    [SerializeField] private Transform camTransform; // Drag Main Camera here
    [SerializeField] private float sensitivity = 0.1f;
    private Vector2 lookInput;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnMove(InputAction.CallbackContext context) => moveInput = context.ReadValue<Vector2>();

    // New Look Event
    public void OnLook(InputAction.CallbackContext context) => lookInput = context.ReadValue<Vector2>();

    void Update()
    {
        // Handle Looking
        float mouseX = lookInput.x * sensitivity;
        float mouseY = lookInput.y * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevents back-flips

        camTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Rotate cam up/down
        transform.Rotate(Vector3.up * mouseX); // Rotate player body left/right

        // Handle Moving
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        
        if (move.magnitude > 1) move.Normalize();

        controller.Move(move * speed * Time.deltaTime);
    }
}