using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private bool isDead = false;

    [SerializeField] private ParticleSystem clickEffect;

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
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * agent.angularSpeed / 10);
        }
    }

    void UpdateAnimation()
    {
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
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
}

