using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = -20f; // Stronger gravity feels better in FP
    private CharacterController controller;
    private Vector2 moveInput;
    public Animator animator;

    [Header("Camera")]
    [SerializeField] private Transform camTransform; 
    [SerializeField] private float sensitivity = 0.1f;
    private Vector2 lookInput;
    private float xRotation = 0f;

    // Gravity Tracking
    private Vector3 velocity; 

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        animator = GetComponentInChildren<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context) => moveInput = context.ReadValue<Vector2>();
    public void OnLook(InputAction.CallbackContext context) => lookInput = context.ReadValue<Vector2>();

    void Update()
    {
        // --- 1. Animation & Looking ---
        float currentSpeed = moveInput.magnitude;
        animator.SetFloat("Speed", currentSpeed);

        float mouseX = lookInput.x * sensitivity;
        float mouseY = lookInput.y * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // --- 2. Horizontal Movement ---
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        if (move.magnitude > 1) move.Normalize();

        // Move horizontally
        controller.Move(move * speed * Time.deltaTime);

        // --- 3. Vertical Movement (Gravity) ---
        // Reset downward force when touching ground so it doesn't build up to infinity
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        // Apply gravity over time
        velocity.y += gravity * Time.deltaTime;

        // Move vertically
        controller.Move(velocity * Time.deltaTime);
    }
}