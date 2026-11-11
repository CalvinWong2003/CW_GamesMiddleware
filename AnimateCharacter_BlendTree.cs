using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateCharacter_BlendTree : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 10.0f;

    private float animator_Vertical, animator_Horizontal;


    private CharacterController characterController;
    private Animator animator;
    void Start()
    {
        // Cache references to the components attached to this GameObject
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 moveDirection = Vector3.zero;

        Keyboard currentKeyboard = Keyboard.current;

        // If no keyboard is connected, skip processing
        if (currentKeyboard == null) return;

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

        //Handles walking and running animations based on movement input
        if (currentKeyboard.leftShiftKey.isPressed)
        {
            moveSpeed = 10.0f; // Increase speed when Left Shift is held down
            animator_Vertical = 1f;
            animator_Horizontal = 0f;

        }
        else
        {
            moveSpeed = 5.0f; // Reset to normal speed when Left Shift is released
            animator_Vertical = 0.5f;
            animator_Horizontal= 0f;
        }

        // Normalize vector for consistent diagonal speed
        if (moveDirection.magnitude > 1.0f)
        {
            moveDirection.Normalize();
        }

        // Move the character
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Handle Rotation and Animation states
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (animator != null)
            {
                animator.SetFloat("Vertical", animator_Vertical);
                animator.SetFloat("Horizontal", animator_Horizontal);
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetFloat("Vertical", animator_Vertical);
                animator.SetFloat("Horizontal", animator_Horizontal);
            }
        }
    }
}
