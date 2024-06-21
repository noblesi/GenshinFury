using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private bool isDead = false;

    [SerializeField] private ParticleSystem clickEffect;

    private bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isDead)
        {
            HandleMouseInput();
            UpdateAnimation();
            SmoothRotate();
            HandleStopping();
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(1)) // 마우스 우클릭 감지
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                agent.SetDestination(hitInfo.point);
                ShowClickEffect(hitInfo.point);
            }
        }

        if (Input.GetMouseButtonDown(0)) // 마우스 좌클릭 감지
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                Attack(hitInfo.point);
            }
        }
    }

    void ShowClickEffect(Vector3 position)
    {
        if(clickEffect != null)
        {
            ParticleSystem effect = Instantiate(clickEffect, position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constantMax);
        }
    }

    void SmoothRotate()
    {
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            Vector3 direction = agent.steeringTarget - transform.position;
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * agent.angularSpeed / 10);
        }
    }

    void UpdateAnimation()
    {
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }

    void HandleStopping()
    {
        // Stop the agent when it is close enough to the destination
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

            Vector3 direction = targetPosition - transform.position;
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f); // Rotate instantly to the target

            animator.SetTrigger("Attack");
            Invoke("ResetAttack", 1.0f); // Adjust the time to match the attack animation duration
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
    }
}

