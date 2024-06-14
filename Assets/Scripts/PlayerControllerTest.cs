using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Walking,
    Running,
    Attacking,
    Dodging,
    Sprinting
}

public class PlayerControllerTest : MonoBehaviour
{
    // Current state of the player
    private PlayerState currentState;

    // Movement speeds
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float sprintSpeed = 8f;
    private float currentSpeed;

    // Animator component
    private Animator animator;

    // Rigidbody component
    private Rigidbody rb;

    // Camera reference
    public Transform cameraTransform;

    // Flag to toggle walking and running
    private bool isWalking = false;

    // Timer for mouse button hold
    private float rightMouseHoldTime = 0f;
    private float sprintThreshold = 0.5f; // Threshold time to switch to sprint

    private float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    void Start()
    {
        // Initialize components
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // Start in Idle state
        currentState = PlayerState.Idle;
    }

    void Update()
    {
        // Handle state transitions based on player input
        HandleStateTransitions();

        // Execute behaviors based on current state
        ExecuteStateBehaviors();
    }

    void HandleStateTransitions()
    {
        // Toggle walking and running with left control key
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isWalking = !isWalking;
            currentSpeed = isWalking ? walkSpeed : runSpeed;
        }

        // Track right mouse button hold time
        if (Input.GetMouseButton(1))
        {
            rightMouseHoldTime += Time.deltaTime;
        }
        else
        {
            rightMouseHoldTime = 0f;
        }

        switch (currentState)
        {
            case PlayerState.Idle:
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                {
                    ChangeState(isWalking ? PlayerState.Walking : PlayerState.Running);
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    ChangeState(PlayerState.Attacking);
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    if (rightMouseHoldTime < sprintThreshold)
                    {
                        ChangeState(PlayerState.Dodging);
                    }
                }
                break;

            case PlayerState.Walking:
            case PlayerState.Running:
                if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
                {
                    ChangeState(PlayerState.Idle);
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    ChangeState(PlayerState.Attacking);
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    if (rightMouseHoldTime < sprintThreshold)
                    {
                        ChangeState(PlayerState.Dodging);
                    }
                }
                else if (Input.GetMouseButton(1) && rightMouseHoldTime >= sprintThreshold)
                {
                    ChangeState(PlayerState.Sprinting);
                }
                break;

            //case PlayerState.Attacking:
            //    // Return to Idle after attacking (could be based on animation end)
            //    if (/* check if attack animation is finished */)
            //    {
            //        ChangeState(PlayerState.Idle);
            //    }
            //    break;

            //case PlayerState.Dodging:
            //    // Return to Idle after dodging (could be based on animation end)
            //    if (/* check if dodge animation is finished */)
            //    {
            //        ChangeState(PlayerState.Idle);
            //    }
            //    break;

            case PlayerState.Sprinting:
                if (!Input.GetMouseButton(1) || rightMouseHoldTime < sprintThreshold)
                {
                    ChangeState(PlayerState.Idle);
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    ChangeState(PlayerState.Attacking);
                }
                break;
        }
    }

    void ExecuteStateBehaviors()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("isSprinting", false);
                break;

            case PlayerState.Walking:
                MovePlayer(walkSpeed);
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
                animator.SetBool("isSprinting", false);
                break;

            case PlayerState.Running:
                MovePlayer(runSpeed);
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);
                animator.SetBool("isSprinting", false);
                break;

            case PlayerState.Attacking:
                animator.SetTrigger("Attack");
                break;

            case PlayerState.Dodging:
                animator.SetTrigger("Dodge");
                break;

            case PlayerState.Sprinting:
                MovePlayer(sprintSpeed);
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("isSprinting", true);
                break;
        }
    }

    void MovePlayer(float speed)
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Calculate target angle based on camera direction
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            // Rotate player towards the target direction
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Move player in the direction
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.MovePosition(rb.position + moveDirection * speed * Time.deltaTime);
        }
    }

    void ChangeState(PlayerState newState)
    {
        currentState = newState;
    }
}
