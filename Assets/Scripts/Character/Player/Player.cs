using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseCharacter
{
    [SerializeField] private Animator animator;

    protected BasePlayerClass currentClass;
    protected Dictionary<KeyCode, float> skillCooldownTimers = new Dictionary<KeyCode, float>();

    // Current stats
    protected int baseMana;
    protected int baseStrength;
    protected int baseAgility;
    protected int baseIntelligence;
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

    private const float BuffDuration = 10f;

    protected override void Start()
    {
        base.Start();
        Initialize();
    }

    public virtual void Initialize()
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
        var keys = new List<KeyCode>(skillCooldownTimers.Keys);
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

        maxHealth = currentClass.stats.initialHealth;
        baseMana = currentClass.stats.initialMana;
        baseStrength = currentClass.stats.initialStrength;
        baseAgility = currentClass.stats.initialAgility;
        baseIntelligence = currentClass.stats.initialIntelligence;

        UpdateStats();
    }

    protected void UpdateStats()
    {
        currentHealth = maxHealth;
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
        commonSkills = new List<SkillData>(Resources.LoadAll<SkillData>("Data/Skills/Common"));
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

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo))
        {
            Vector3 targetPosition = hitInfo.point;
            ExecuteSkill(targetPosition, skill);
        }
    }

    protected void ExecuteSkill(Vector3 targetPosition, SkillData skill)
    {
        RotateTowards(targetPosition);

        if (skill.effect != null)
        {
            PlaySkillEffect(targetPosition, skill.effect);
        }

        animator.SetTrigger(skill.animationTrigger);
        skillCooldownTimers[skill.key] = skill.GetCooldown();

        ApplySkillEffects(targetPosition, skill);
    }

    private void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f);
    }

    private void PlaySkillEffect(Vector3 targetPosition, ParticleSystem effect)
    {
        ParticleSystem instantiatedEffect = Instantiate(effect, targetPosition, Quaternion.identity);
        instantiatedEffect.Play();
        Destroy(instantiatedEffect.gameObject, instantiatedEffect.main.duration + instantiatedEffect.main.startLifetime.constantMax);
    }

    protected void ApplySkillEffects(Vector3 targetPosition, SkillData skill)
    {
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
                    ApplyMeleeAttack(skill);
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

    protected void ApplyMeleeAttack(SkillData skill)
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

        yield return new WaitForSeconds(BuffDuration);

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

    protected override void Die()
    {
        Debug.Log("Player Died");
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
