using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseCharacter : MonoBehaviour, IDamageable
{
    public int maxHealth { get; protected set; }
    public int currentHealth { get; protected set; }
    public int maxMana { get; protected set; }
    public int currentMana { get; protected set; }
    protected bool isDebuffed;
    protected float debuffDuration;
    protected NavMeshAgent agent;

    public event System.Action OnHealthChanged;
    public event System.Action OnManaChanged;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();

        // 캐릭터의 Collider와 Rigidbody 설정
        EnsureComponent<Collider>(gameObject);
        EnsureRigidbody(gameObject);
    }

    public virtual void TakeDamage(int damage, DamageType damageType)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke();

        Debug.Log($"{gameObject.name} took {damage} {damageType} damage. Current health: {currentHealth}");

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

    public virtual void ApplyDebuff(SkillData debuffSkill)
    {
        if (!isDebuffed)
        {
            StartCoroutine(ApplyDebuffCoroutine(debuffSkill));
        }
    }

    private IEnumerator ApplyDebuffCoroutine(SkillData debuffSkill)
    {
        isDebuffed = true;
        debuffDuration = debuffSkill.GetCooldown();

        float originalSpeed = agent.speed;
        agent.speed *= 0.5f;

        yield return new WaitForSeconds(debuffDuration);

        agent.speed = originalSpeed;
        isDebuffed = false;
    }

    protected void EnsureRigidbody(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = obj.AddComponent<Rigidbody>();
            rb.isKinematic = true; // 물리 엔진의 영향을 받지 않도록 설정
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
