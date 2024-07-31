using UnityEngine;
using UnityEngine.AI;

public abstract class BaseCharacter : MonoBehaviour, IDamageable
{
    public int maxHealth { get; protected set; }
    public int currentHealth { get; protected set; }
    public int maxMana { get; protected set; }
    public int currentMana { get; protected set; }
    protected NavMeshAgent agent;

    public event System.Action OnHealthChanged;
    public event System.Action OnManaChanged;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        EnsureComponent<Collider>(gameObject);
        EnsureRigidbody(gameObject);
    }

    public virtual void TakeDamage(int damage)
    {
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

    protected abstract void Die();
}
