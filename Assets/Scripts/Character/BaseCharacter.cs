using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseCharacter : MonoBehaviour, IDamageable
{
    public int maxHealth { get; protected set; }
    public int currentHealth { get; protected set; }
    public int maxMana { get; protected set; }
    public int currentMana {get; protected set; }
    protected bool isDebuffed;
    protected float debuffDuration;
    protected NavMeshAgent agent;

    public event System.Action OnHealthChanged;
    public event System.Action OnManaChanged;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
    }

    public virtual void TakeDamage(int damage, DamageType damageType)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(); // 이벤트 호출

        Debug.Log($"{gameObject.name} took {damage} {damageType} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected void ChangeMana(int mana)
    {
        currentMana = mana;
        OnManaChanged?.Invoke();
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

    protected abstract void Die();
}
