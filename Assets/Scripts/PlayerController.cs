using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    private float vertical;

    private bool isRunning = true; // 기본값은 Run
    private bool isSprinting = false;

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
            isRunning = !isRunning; // Run과 Walk 상태를 토글
            playerData.CurrentSpeed = isRunning ? playerData.RunSpeed : playerData.WalkSpeed;
        }

        if (Input.GetMouseButton(1) && playerData.Stamina > 0)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        Move();
        HandleStamina();
    }

    private void Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        moveVector = new Vector3(horizontal, 0, vertical).normalized;

        // 이동 속도 설정
        float speed = playerData.CurrentSpeed;
        if (isSprinting)
        {
            speed = playerData.SprintSpeed;
        }

        transform.position += moveVector * speed * Time.deltaTime;

        animator.SetFloat("Speed", moveVector.magnitude * speed / playerData.SprintSpeed);

        if (moveVector != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveVector, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 10f);
        }
    }

    private void HandleStamina()
    {
        if (isSprinting)
        {
            playerData.Stamina -= playerData.StaminaDecreaseRate * Time.deltaTime;
            if (playerData.Stamina == 0)
            {
                isSprinting = false;
            }
        }
        else
        {
            playerData.Stamina += playerData.StaminaRecoveryRate * Time.deltaTime;
        }
    }
}
