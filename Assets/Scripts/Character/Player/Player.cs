using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator animator;

    protected BasePlayerClass currentClass;
    protected Dictionary<KeyCode, float> skillCooldownTimers = new Dictionary<KeyCode, float>();

    // Current stats
    protected int baseHealth;
    protected int baseMana;
    protected int baseStrength;
    protected int baseAgility;
    protected int baseIntelligence;
    protected int currentHealth;
    protected int currentMana;
    protected int currentStrength;
    protected int currentAgility;
    protected int currentIntelligence;
    protected int currentLevel = 1;

    // Equipment slots
    public EquipmentItem helmet;
    public EquipmentItem armor;
    public EquipmentItem gloves;
    public EquipmentItem boots;
    public EquipmentItem weapon;

    // Skill books
    public List<SkillData> commonSkills = new List<SkillData>();

    public virtual void Initialize(GameData gameData)
    {
        currentClass = null;
        InitializeStats();
        InitializeCooldownTimers();
        LoadCommonSkills();
    }

    void Update()
    {
        UpdateCooldownTimers();
        HandleSkillInput();
    }

    protected void InitializeCooldownTimers()
    {
        skillCooldownTimers.Clear();
        foreach (var skill in commonSkills)
        {
            skillCooldownTimers[skill.key] = 0f;
        }
    }

    protected void UpdateCooldownTimers()
    {
        List<KeyCode> keys = new List<KeyCode>(skillCooldownTimers.Keys);
        foreach (var key in keys)
        {
            if (skillCooldownTimers[key] > 0)
            {
                skillCooldownTimers[key] -= Time.deltaTime;
            }
        }
    }

    protected void InitializeStats()
    {
        if (currentClass == null) return;

        baseHealth = currentClass.stats.initialHealth;
        baseMana = currentClass.stats.initialMana;
        baseStrength = currentClass.stats.initialStrength;
        baseAgility = currentClass.stats.initialAgility;
        baseIntelligence = currentClass.stats.initialIntelligence;

        UpdateStats();
    }

    protected void UpdateStats()
    {
        currentHealth = baseHealth;
        currentMana = baseMana;
        currentStrength = baseStrength;
        currentAgility = baseAgility;
        currentIntelligence = baseIntelligence;

        ApplyEquipmentStats(helmet);
        ApplyEquipmentStats(armor);
        ApplyEquipmentStats(gloves);
        ApplyEquipmentStats(boots);
        ApplyEquipmentStats(weapon);
    }

    protected void ApplyEquipmentStats(EquipmentItem equipment)
    {
        if (equipment == null) return;

        currentHealth += equipment.BaseHealth;
        currentMana += equipment.BaseAttack;
        currentStrength += equipment.BaseAttack;
        currentAgility += equipment.BaseAgility;
        currentIntelligence += equipment.BaseIntelligence;

        currentHealth += equipment.GetStat(StatType.Health);
        currentMana += equipment.GetStat(StatType.Mana);
        currentStrength += equipment.GetStat(StatType.Strength);
        currentAgility += equipment.GetStat(StatType.Agility);
        currentIntelligence += equipment.GetStat(StatType.Intelligence);
    }

    protected void LoadCommonSkills()
    {
        commonSkills = new List<SkillData>(Resources.LoadAll<SkillData>("Skills/Common"));
    }

    public virtual void HandleSkillInput()
    {
        foreach (var skill in commonSkills)
        {
            if (Input.GetKeyDown(skill.key) && skillCooldownTimers[skill.key] <= 0)
            {
                UseSkill(skill);
            }
        }
    }

    protected void UseSkill(SkillData skill)
    {
        if (currentMana < skill.GetManaCost())
        {
            Debug.Log("Not enough mana!");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 targetPosition = hitInfo.point;
            ExecuteSkill(targetPosition, skill);
        }
    }

    protected void ExecuteSkill(Vector3 targetPosition, SkillData skill)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f);

        if (skill.effect != null)
        {
            ParticleSystem effect = Instantiate(skill.effect, targetPosition, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constantMax);
        }

        animator.SetTrigger(skill.animationTrigger);
        skillCooldownTimers[skill.key] = skill.GetCooldown();

        ApplySkillEffects(targetPosition, skill);
    }

    protected void ApplySkillEffects(Vector3 targetPosition, SkillData skill)
    {
        float damage = skill.GetDamage();
        float manaCost = skill.GetManaCost();

        if (currentMana < manaCost)
        {
            Debug.Log("Not enough mana!");
            return;
        }

        currentMana -= (int)manaCost;

        foreach (var type in skill.types)
        {
            switch (type)
            {
                case SkillType.Melee:
                    ApplyMeleeAttack(targetPosition, skill);
                    break;
                case SkillType.Ranged:
                    ApplyRangedAttack(targetPosition, skill);
                    break;
                case SkillType.SingleTarget:
                    ApplySingleTargetAttack(targetPosition, skill);
                    break;
                case SkillType.AreaOfEffect:
                    ApplyAreaOfEffectAttack(targetPosition, skill);
                    break;
                case SkillType.Buff:
                    ApplyBuff(skill);
                    break;
                case SkillType.Debuff:
                    ApplyDebuff(targetPosition, skill);
                    break;
            }
        }
    }

    protected void ApplyMeleeAttack(Vector3 targetPosition, SkillData skill)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, skill.range);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                hitCollider.GetComponent<Enemy>().TakeDamage((int)skill.GetDamage(), skill.damageType);
                break;
            }
        }
    }

    protected void ApplyRangedAttack(Vector3 targetPosition, SkillData skill)
    {
        Ray ray = new Ray(transform.position, targetPosition - transform.position);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, skill.range))
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                hitInfo.collider.GetComponent<Enemy>().TakeDamage((int)skill.GetDamage(), skill.damageType);
            }
        }
    }

    protected void ApplySingleTargetAttack(Vector3 targetPosition, SkillData skill)
    {
        Ray ray = new Ray(transform.position, targetPosition - transform.position);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, skill.range))
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                hitInfo.collider.GetComponent<Enemy>().TakeDamage((int)skill.GetDamage(), skill.damageType);
            }
        }
    }

    protected void ApplyAreaOfEffectAttack(Vector3 targetPosition, SkillData skill)
    {
        Collider[] hitColliders = Physics.OverlapSphere(targetPosition, skill.areaRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                hitCollider.GetComponent<Enemy>().TakeDamage((int)skill.GetDamage(), skill.damageType);
            }
        }
    }

    protected void ApplyBuff(SkillData skill)
    {
        StartCoroutine(ApplyBuffCoroutine(skill));
    }

    protected IEnumerator ApplyBuffCoroutine(SkillData skill)
    {
        int originalStrength = currentStrength;
        int originalAgility = currentAgility;

        currentStrength += (int)(originalStrength * 0.2f);
        currentAgility += (int)(originalAgility * 0.2f);

        yield return new WaitForSeconds(10f);

        currentStrength = originalStrength;
        currentAgility = originalAgility;
    }

    protected void ApplyDebuff(Vector3 targetPosition, SkillData skill)
    {
        Collider[] hitColliders = Physics.OverlapSphere(targetPosition, skill.areaRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                hitCollider.GetComponent<Enemy>().ApplyDebuff(skill);
            }
        }
    }

    public bool LevelUpSkill(string skillName)
    {
        return currentClass.LevelUpSkill(skillName);
    }

    public void AddSkillPoint()
    {
        currentClass.AddSkillPoint();
    }

    public void LevelUp()
    {
        currentLevel++;
        currentClass.ApplyLevelUp(ref currentHealth, ref currentMana, ref currentStrength, ref currentAgility, ref currentIntelligence);
    }
}
