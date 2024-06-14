using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    private float vertical;

    private bool isMoving = false;
    private bool isRunning = true; // �⺻���� Run
    private bool sprintOnce = false;
    private bool isSprinting = false;

    private float targetSpeed = 0f;
    private float currentSpeed = 0f;
    private float smoothTime = 0.1f;
    private float speedSmoothVelocity = 0f;

    private bool sprintCooldown = false;
    private float sprintCooldownTime = 5f;

    Vector3 moveVector = Vector3.zero;

    private Animator animator;
    private PlayerData playerData;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerData = GetComponent<PlayerData>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isRunning = !isRunning; // Run�� Walk ���¸� ���
            playerData.CurrentSpeed = isRunning ? playerData.RunSpeed : playerData.WalkSpeed;
        }

        if(!sprintCooldown && Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("SprintOnce");
        }

        if (!sprintCooldown && Input.GetMouseButton(1) && playerData.Stamina > 0)
        {
            isSprinting = true;
        }
        else if(!Input.GetMouseButton(1))
        {
            isSprinting = false;
        }

        Move();
        HandleStamina();
        UpdateAnimator();
    }

    private void Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        moveVector = new Vector3(horizontal, 0, vertical).normalized;

        isMoving = moveVector != Vector3.zero;

        // �̵� �ӵ� ����
        targetSpeed = playerData.CurrentSpeed;
        if (isSprinting)
        {
            targetSpeed = playerData.SprintSpeed;
        }
        else if (!isMoving)
        {
            targetSpeed = playerData.IdleSpeed;
        }

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, smoothTime);
        transform.position += moveVector * currentSpeed * Time.deltaTime;

        if (isMoving)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveVector, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 10f);
        }
    }

    private void HandleStamina()
    {
        if (isSprinting)
        {
            playerData.DecreaseStamina(playerData.StaminaDecreaseRate * Time.deltaTime);
            if (playerData.Stamina == 0)
            {
                isSprinting = false;
                isRunning = true;
                playerData.CurrentSpeed = playerData.RunSpeed;
                StartCoroutine(SprintCooldown());
            }
        }
        else
        {
            playerData.RecoverStamina(playerData.StaminaRecoveryRate * Time.deltaTime);
        }
    }

    private void UpdateAnimator()
    {
        //float speedPercent = currentSpeed / playerData.SprintSpeed;
        animator.SetFloat("Speed", currentSpeed);
        animator.SetBool("isMove", isMoving);
    }

    private IEnumerator SprintCooldown()
    {
        sprintCooldown = true;
        yield return new WaitForSeconds(sprintCooldownTime);
        sprintCooldown = false;
    }
}
