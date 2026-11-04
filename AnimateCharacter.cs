using UnityEngine;
// 1. You must include the New Input System namespace
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class AnimateCharacter : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 10.0f;
    
    [Header("Jump & Gravity Settings")]
    public float jumpHeight = 2.0f;
    public float gravity = -9.81f; // Custom gravity multiplier (Unity default is -9.81f)
    
    private CharacterController characterController;
    private Animator animator;
    
    private Vector3 verticalVelocity; // Tracks gravity and jump force
    private bool isGrounded;
    
    // Animator parameter names (Make sure these match your Animator Controller exactly)
    private const string IS_WALKING_PARAM = "isWalking";
    private const string JUMPING_TRIGGER_PARAM = "Jumping"; //Trigger parameter

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Cache references to the components attached to this GameObject
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = characterController.isGrounded;
        if (isGrounded && verticalVelocity.y < 0)
        {
            // Reset velocity, but keep a tiny downward force so isGrounded stays stable
            verticalVelocity.y = -2f;
        }
        Vector3 moveDirection = Vector3.zero;

        // 2. Access the current keyboard layout directly
        Keyboard currentKeyboard = Keyboard.current;

        // If no keyboard is connected, skip processing
        if (currentKeyboard == null) return;

        // 3. Explicitly check key states using the new system
        if (currentKeyboard.wKey.isPressed)
        {
            moveDirection += Vector3.forward;
        }
        if (currentKeyboard.sKey.isPressed)
        {
            moveDirection += Vector3.back;
        }
        if (currentKeyboard.aKey.isPressed)
        {
            moveDirection += Vector3.left;
        }
        if (currentKeyboard.dKey.isPressed)
        {
            moveDirection += Vector3.right;
        }
        // Handle Jump Input (Spacebar)
        if (currentKeyboard.spaceKey.wasPressedThisFrame && isGrounded)
        {
            // Formula to calculate upward force based on target height: physics equation v = sqrt(h * -2 * g)
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // Trigger Jump Animation
            if (animator != null)
            {
                animator.SetTrigger(JUMPING_TRIGGER_PARAM);
            }
        }

        // Normalize vector for consistent diagonal speed
        if (moveDirection.magnitude > 1.0f)
        {
            moveDirection.Normalize();
        }

        // Move the character
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        //Applying gravity over time and move vertically
        verticalVelocity.y += gravity * Time.deltaTime;
        characterController.Move(verticalVelocity * Time.deltaTime);

        // Handle Rotation and Animation states
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (animator != null)
            {
                animator.SetBool(IS_WALKING_PARAM, true);
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool(IS_WALKING_PARAM, false);
            }
        }
    }
}
