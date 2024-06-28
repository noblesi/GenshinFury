using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private bool isDead = false;
    [SerializeField] private ParticleSystem clickEffect;
    [SerializeField] private Player playerSkills;

    private bool isAttacking = false;
    private bool isRotating = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        animator = GetComponent<Animator>();
        playerSkills = GetComponent<Player>();
    }

    void Update()
    {
        if (!isDead)
        {
            UpdateMouseInput();
            UpdateAnimation();
            UpdateRotation();
            UpdateStopping();
            playerSkills.HandleSkillInput();
        }
    }

    void UpdateMouseInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            HandleRightClick();
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleLeftClick();
        }
    }

    void HandleRightClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            agent.SetDestination(hitInfo.point);
            ShowClickEffect(hitInfo.point);
        }
    }

    void HandleLeftClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Attack(hitInfo.point);
        }
    }

    void ShowClickEffect(Vector3 position)
    {
        if (clickEffect != null)
        {
            ParticleSystem effect = Instantiate(clickEffect, position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constantMax);
        }
    }

    void UpdateRotation()
    {
        if (!isAttacking && agent.remainingDistance > agent.stoppingDistance)
        {
            Vector3 direction = agent.steeringTarget - transform.position;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * agent.angularSpeed / 10);
            }
        }
    }

    void UpdateAnimation()
    {
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }

    void UpdateStopping()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.velocity = Vector3.zero;
            animator.SetFloat("Speed", 0);
        }
    }

    public void TakeDamage()
    {
        animator.SetTrigger("Hit");
    }

    public void Die()
    {
        isDead = true;
        agent.isStopped = true;
        animator.SetTrigger("Dead");
    }

    void Attack(Vector3 targetPosition)
    {
        if (!isAttacking)
        {
            isAttacking = true;
            agent.isStopped = true;
            agent.ResetPath();
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            StartCoroutine(RotateAndAttack(targetRotation));
        }
    }

    private IEnumerator RotateAndAttack(Quaternion targetRotation)
    {
        isRotating = true;
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * agent.angularSpeed / 10);
            yield return null;
        }
        isRotating = false;
        animator.SetTrigger("Attack");
        Invoke("ResetAttack", 1.0f);
    }

    void ResetAttack()
    {
        isAttacking = false;
        agent.isStopped = false;
        animator.SetFloat("Speed", 0);
    }
}
