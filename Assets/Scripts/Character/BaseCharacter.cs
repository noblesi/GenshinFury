using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseCharacter : MonoBehaviour, IDamageable
{
    public int maxHealth;
    protected int currentHealth;
    protected bool isDebuffed;
    protected float debuffDuration;
    protected NavMeshAgent agent;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
    }

    public virtual void TakeDamage(int damage, DamageType damageType)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} {damageType} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
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
