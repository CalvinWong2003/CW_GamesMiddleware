using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class NetworkingwithAudio : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5.0f;
    public float turnSpeed = 150.0f; // Degrees per second for A/D turning

    private float animator_Vertical, animator_Horizontal;
    private CharacterController characterController;

    [Header("Attachment & Throwing")]
    public Transform holdPoint; // Drag your empty Hand HoldPoint here
    public float throwForce = 15f;
    public float upwardArc = 2f;

    [Header("Audio & Animation Events")]
    public AudioClip[] footstepClips; // Assign multiple footstep sounds for variety
    public AudioClip throwClip;
    public AudioClip grabClip;
    private AudioSource audioSource;

    private Animator animator;
    private GameObject nearbyBall;
    private GameObject heldBall;
    private Rigidbody heldBallRb;

    void Start()
    {
        // Cache references to the components attached to this GameObject
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Keyboard currentKeyboard = Keyboard.current;

        // If no keyboard is connected, skip processing
        if (currentKeyboard == null)
        {
            animator_Vertical = 0f;
            animator_Horizontal = 0f;
            return;
        }

        // Reset values at the start of each frame
        animator_Vertical = 0f;
        animator_Horizontal = 0f;
        float turnInput = 0f;
        Vector3 moveDirection = Vector3.zero;

        // 1. Handle Turning (A and D) - Triggers Turn Blend Tree without Walk/Sprint
        if (currentKeyboard.aKey.isPressed)
        {
            turnInput = -1.0f;
            animator_Horizontal = -1.0f;
        }
        else if (currentKeyboard.dKey.isPressed)
        {
            turnInput = 1.0f;
            animator_Horizontal = 1.0f;
        }

        // Apply rotation based on A/D input
        if (turnInput != 0f)
        {
            transform.Rotate(0f, turnInput * turnSpeed * Time.deltaTime, 0f);
        }

        // 2. Handle Forward Movement (W) - Moves in the direction the character is facing
        if (currentKeyboard.wKey.isPressed)
        {
            moveDirection = transform.forward;

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

        // Move the character forward (only applies when W is pressed)
        if (moveDirection != Vector3.zero)
        {
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }

        // 3. Update Animator Parameters
        if (animator != null)
        {
            // Dampening (0.1f) smooths the transition between idle, turn, and walk/sprint
            animator.SetFloat("Vertical", animator_Vertical, 0.1f, Time.deltaTime);
            animator.SetFloat("Horizontal", animator_Horizontal, 0.1f, Time.deltaTime);
        }

        // 4. Interaction & Throwing Triggers
        // Use wasPressedThisFrame so the trigger only fires once per key press
        if (heldBall == null && nearbyBall != null && currentKeyboard.eKey.wasPressedThisFrame)
        {
            animator.SetTrigger("Interact");
        }
        else if (heldBall != null && currentKeyboard.rKey.wasPressedThisFrame)
        {
            animator.SetTrigger("Throw");
        }
    }

    // =========================================================================
    // ANIMATION EVENT METHODS
    // Call these exact function names from your Animation Clips in Unity!
    // =========================================================================

    // Triggered when a foot touches the ground in Walk, Sprint, or Turn animations
    public void OnFootstep(AnimationEvent animationEvent)
    {
        // Prevents blended-out animations from triggering double footsteps
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (footstepClips != null && footstepClips.Length > 0 && audioSource != null)
            {
                // Pick a random clip and slightly vary pitch for natural sound
                AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(clip);
            }
        }
    }

    // Triggered at the exact frame the hand touches the ball in the Interact animation
    public void OnGrabBall()
    {
        if (nearbyBall == null) return;

        heldBall = nearbyBall;
        heldBallRb = heldBall.GetComponent<Rigidbody>();

        if (heldBallRb != null)
        {
            heldBallRb.isKinematic = true;
        }

        heldBall.transform.SetParent(holdPoint);
        heldBall.transform.localPosition = Vector3.zero;
        heldBall.transform.localRotation = Quaternion.identity;

        if (grabClip != null && audioSource != null)
        {
            audioSource.pitch = 1.0f;
            audioSource.PlayOneShot(grabClip);
        }
    }

    // Triggered at the exact frame the arm extends forward in the Throw animation
    public void OnReleaseBall()
    {
        if (heldBall == null) return;

        heldBall.transform.SetParent(null);

        if (heldBallRb != null)
        {
            heldBallRb.isKinematic = false;
            Vector3 throwDirection = (transform.forward * throwForce) + (Vector3.up * upwardArc);
            heldBallRb.AddForce(throwDirection, ForceMode.Impulse);
        }

        if (throwClip != null && audioSource != null)
        {
            audioSource.pitch = 1.0f;
            audioSource.PlayOneShot(throwClip);
        }

        heldBall = null;
        heldBallRb = null;
    }

    // =========================================================================
    // TRIGGER DETECTION
    // =========================================================================

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
