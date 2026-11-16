using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkingCharacter : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 10.0f;

    private float animator_Vertical, animator_Horizontal;
    private GameObject currentInteractable;

    private CharacterController characterController;
    
    [Header("Attachment & Throwing")]
    public Transform holdPoint; // Drag your empty Hand HoldPoint here
    public float throwForce = 15f;
    public float upwardArc = 2f;

    private Animator animator;
    private GameObject nearbyBall;
    private GameObject heldBall;
    private Rigidbody heldBallRb;

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
        if (currentKeyboard == null)
        {
            animator_Vertical = 0f;
            return;
        }

        //Handles walking and running animations based on movement input
        if (currentKeyboard.wKey.isPressed)
        {
            moveDirection += Vector3.forward;
            if (currentKeyboard.leftShiftKey.isPressed)
            {
                moveSpeed = 10.0f; // Increase speed when Left Shift is held down
                animator_Vertical = 1.0f; // Set to running animation
            }
            else
            {
                moveSpeed = 5.0f; // Reset to normal speed when Left Shift is released
                animator_Vertical = 0.5f; // Set to walking animation
            }
        }
        else if(currentKeyboard.sKey.isPressed)
        {
            moveDirection += Vector3.back;
            if (currentKeyboard.leftShiftKey.isPressed)
            {
                moveSpeed = 10.0f; // Increase speed when Left Shift is held down
                animator_Vertical = 1.0f; // Set to running animation
            }
            else
            {
                moveSpeed = 5.0f; // Reset to normal speed when Left Shift is released
                animator_Vertical = 0.5f; // Set to walking animation
            }
        }
        else if (currentKeyboard.aKey.isPressed)
        {
            moveDirection += Vector3.left;
            if (currentKeyboard.leftShiftKey.isPressed)
            {
                moveSpeed = 10.0f; // Increase speed when Left Shift is held down
                animator_Vertical = 1.0f; // Set to running animation
            }
            else
            {
                moveSpeed = 5.0f; // Reset to normal speed when Left Shift is released
                animator_Vertical = 0.5f; // Set to walking animation
            }
        }
        else if (currentKeyboard.dKey.isPressed)
        {
            moveDirection += Vector3.right;
            if (currentKeyboard.leftShiftKey.isPressed)
            {
                moveSpeed = 10.0f; // Increase speed when Left Shift is held down
                animator_Vertical = 1.0f; // Set to running animation
            }
            else
            {
                moveSpeed = 5.0f; // Reset to normal speed when Left Shift is released
                animator_Vertical = 0.5f; // Set to walking animation
            }
        }
        else
        {
            animator_Vertical = 0f; // Reset to idle when no movement keys are pressed
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
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetFloat("Vertical", animator_Vertical);
            }
        }

        // Press E to pick up the ball if hands are empty
        if (heldBall == null && nearbyBall != null && currentKeyboard.eKey.isPressed)
        {
            PickUpBall();
        }
        // Press R to throw the ball if currently holding one
        else if (heldBall != null && currentKeyboard.rKey.isPressed)
        {
            ThrowBall();
        }
    }
    void PickUpBall()
    {
        animator.SetTrigger("Interact");

        heldBall = nearbyBall;
        heldBallRb = heldBall.GetComponent<Rigidbody>();

        // Turn off physics so the ball doesn't fall out of the character's hands
        if (heldBallRb != null)
        {
            heldBallRb.isKinematic = true;
        }

        // Attach the ball to the hand and snap it to the HoldPoint's exact position
        heldBall.transform.SetParent(holdPoint);
        heldBall.transform.localPosition = Vector3.zero;
        heldBall.transform.localRotation = Quaternion.identity;
    }

    void ThrowBall()
    {
        animator.SetTrigger("Throw");

        // Detach the ball from the character's hand
        heldBall.transform.SetParent(null);

        if (heldBallRb != null)
        {
            // Re-enable physics and launch the ball forward
            heldBallRb.isKinematic = false;
            Vector3 throwDirection = (transform.forward * throwForce) + (Vector3.up * upwardArc);
            heldBallRb.AddForce(throwDirection, ForceMode.Impulse);
        }

        // Clear references so the character can pick up another ball later
        heldBall = null;
        heldBallRb = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable") && heldBall == null)
        {
            nearbyBall = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable") && other.gameObject == nearbyBall)
        {
            nearbyBall = null;
        }
    }
}
