using UnityEngine;
using UnityEngine.AI;

public abstract class BaseCharacter : MonoBehaviour, IDamageable
{
    public int maxHealth { get; protected set; }
    public int currentHealth { get; protected set; }
    public int maxMana { get; protected set; }
    public int currentMana { get; protected set; }
    protected NavMeshAgent agent;
    protected bool isDead = false;

    public event System.Action OnHealthChanged;
    public event System.Action OnManaChanged;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        EnsureComponent<Collider>(gameObject);
        EnsureRigidbody(gameObject);
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        OnHealthChanged?.Invoke();

        Debug.Log($"{gameObject.name} took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ChangeMana(int mana)
    {
        currentMana = mana;
        OnManaChanged?.Invoke();
    }

    public void ChangeHealth(int health)
    {
        currentHealth = health;
        OnHealthChanged?.Invoke();
    }

    protected void EnsureRigidbody(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = obj.AddComponent<Rigidbody>();
            rb.isKinematic = true;
        }
    }

    protected void EnsureComponent<T>(GameObject obj) where T : Component
    {
        if (obj.GetComponent<T>() == null)
        {
            obj.AddComponent<T>();
        }
    }

    protected virtual void Die()
    {
        if (isDead) return;
        isDead = true;
        Debug.Log($"{gameObject.name} has died.");
    }

    protected void PlayAnimation(Animator animator, string triggerName)
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
        }
    }

    protected void ResetAnimationTrigger(Animator animator, string triggerName)
    {
        if (animator != null)
        {
            animator.ResetTrigger(triggerName);
        }
    }
}