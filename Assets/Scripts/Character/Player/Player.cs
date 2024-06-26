using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private BasePlayerClass currentClass;
    private Dictionary<KeyCode, float> skillCooldownTimers = new Dictionary<KeyCode, float>();

    // Current stats
    private int baseHealth;
    private int baseMana;
    private int baseStrength;
    private int baseAgility;
    private int baseIntelligence;
    private int currentHealth;
    private int currentMana;
    private int currentStrength;
    private int currentAgility;
    private int currentIntelligence;
    private int currentLevel = 1;

    // Equipment slots
    public EquipmentItem helmet;
    public EquipmentItem armor;
    public EquipmentItem gloves;
    public EquipmentItem boots;
    public EquipmentItem weapon;

    // Skill books
    public List<SkillData> commonSkills;
    public List<SkillData> warriorSkills;
    public List<SkillData> archerSkills;
    public List<SkillData> wizardSkills;

    public void Initialize(GameData gameData)
    {
        BasePlayerClass playerClass = null;
        switch (gameData.playerClass)
        {
            case PlayerClass.Warrior:
                playerClass = Resources.Load<BasePlayerClass>("PlayerClass/WarriorSkills");
                warriorSkills = new List<SkillData>(Resources.LoadAll<SkillData>("Skills/Warrior"));
                break;
            case PlayerClass.Archer:
                playerClass = Resources.Load<BasePlayerClass>("PlayerClass/ArcherSkills");
                archerSkills = new List<SkillData>(Resources.LoadAll<SkillData>("Skills/Archer"));
                break;
            case PlayerClass.Wizard:
                playerClass = Resources.Load<BasePlayerClass>("PlayerClass/WizardSkills");
                wizardSkills = new List<SkillData>(Resources.LoadAll<SkillData>("Skills/Wizard"));
                break;
            default:
                Debug.LogError("Invalid player class.");
                return;
        }

        if (playerClass != null)
        {
            currentClass = playerClass;
            InitializeStats();
            InitializeCooldownTimers();
        }

        LoadCommonSkills();
    }

    void Update()
    {
        UpdateCooldownTimers();
        HandleSkillInput();
    }

    void InitializeCooldownTimers()
    {
        skillCooldownTimers.Clear();
        foreach (var skill in commonSkills)
        {
            skillCooldownTimers[skill.key] = 0f;
        }
        foreach (var skill in warriorSkills)
        {
            skillCooldownTimers[skill.key] = 0f;
        }
        foreach (var skill in archerSkills)
        {
            skillCooldownTimers[skill.key] = 0f;
        }
        foreach (var skill in wizardSkills)
        {
            skillCooldownTimers[skill.key] = 0f;
        }
    }

    void UpdateCooldownTimers()
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

    void InitializeStats()
    {
        baseHealth = currentClass.stats.initialHealth;
        baseMana = currentClass.stats.initialMana;
        baseStrength = currentClass.stats.initialStrength;
        baseAgility = currentClass.stats.initialAgility;
        baseIntelligence = currentClass.stats.initialIntelligence;

        UpdateStats();
    }

    void UpdateStats()
    {
        currentHealth = baseHealth;
        currentMana = baseMana;
        currentStrength = baseStrength;
        currentAgility = baseAgility;
        currentIntelligence = baseIntelligence;

        // Apply equipment bonuses
        ApplyEquipmentStats(helmet);
        ApplyEquipmentStats(armor);
        ApplyEquipmentStats(gloves);
        ApplyEquipmentStats(boots);
        ApplyEquipmentStats(weapon);
    }

    void ApplyEquipmentStats(EquipmentItem equipment)
    {
        if (equipment == null) return;

        currentHealth += equipment.BaseHealth;
        currentMana += equipment.BaseAttack; // Weapons provide attack power
        currentStrength += equipment.BaseAttack; // Weapons provide attack power
        currentAgility += equipment.BaseAgility;
        currentIntelligence += equipment.BaseIntelligence;

        // Apply random stats
        currentHealth += equipment.GetStat(StatType.Health);
        currentMana += equipment.GetStat(StatType.Mana);
        currentStrength += equipment.GetStat(StatType.Strength);
        currentAgility += equipment.GetStat(StatType.Agility);
        currentIntelligence += equipment.GetStat(StatType.Intelligence);
    }

    void LoadCommonSkills()
    {
        commonSkills = new List<SkillData>(Resources.LoadAll<SkillData>("Skills/Common"));
    }

    public void HandleSkillInput()
    {
        foreach (var skill in commonSkills)
        {
            if (Input.GetKeyDown(skill.key) && skillCooldownTimers[skill.key] <= 0)
            {
                UseSkill(skill);
            }
        }

        if (currentClass.className == "Warrior")
        {
            foreach (var skill in warriorSkills)
            {
                if (Input.GetKeyDown(skill.key) && skillCooldownTimers[skill.key] <= 0)
                {
                    UseSkill(skill);
                }
            }
        }
        else if (currentClass.className == "Archer")
        {
            foreach (var skill in archerSkills)
            {
                if (Input.GetKeyDown(skill.key) && skillCooldownTimers[skill.key] <= 0)
                {
                    UseSkill(skill);
                }
            }
        }
        else if (currentClass.className == "Wizard")
        {
            foreach (var skill in wizardSkills)
            {
                if (Input.GetKeyDown(skill.key) && skillCooldownTimers[skill.key] <= 0)
                {
                    UseSkill(skill);
                }
            }
        }
    }

    void UseSkill(SkillData skill)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Vector3 targetPosition = hitInfo.point;
            ExecuteSkill(targetPosition, skill);
        }
    }

    void ExecuteSkill(Vector3 targetPosition, SkillData skill)
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

    void ApplySkillEffects(Vector3 targetPosition, SkillData skill)
    {
        float damage = skill.GetDamage();
        float manaCost = skill.GetManaCost();

        // Check if player has enough mana
        if (currentMana < manaCost)
        {
            Debug.Log("Not enough mana!");
            return;
        }

        currentMana -= (int)manaCost;

        // Add your skill effect logic here based on skill types and damage type
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

    void ApplyMeleeAttack(Vector3 targetPosition, SkillData skill)
    {
        // Implement melee attack logic with range and damage
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, skill.range);
        foreach (var hitCollider in hitColliders)
        {
            // Assuming enemies have a tag "Enemy"
            if (hitCollider.CompareTag("Enemy"))
            {
                // Apply damage to the enemy
                hitCollider.GetComponent<Enemy>().TakeDamage((int)skill.GetDamage(), skill.damageType);
                break; // Melee attack hits the first enemy in range
            }
        }
    }

    void ApplyRangedAttack(Vector3 targetPosition, SkillData skill)
    {
        // Implement ranged attack logic with range and damage
        Ray ray = new Ray(transform.position, targetPosition - transform.position);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, skill.range))
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                // Apply damage to the enemy
                hitInfo.collider.GetComponent<Enemy>().TakeDamage((int)skill.GetDamage(), skill.damageType);
            }
        }
    }

    void ApplySingleTargetAttack(Vector3 targetPosition, SkillData skill)
    {
        // Implement single target logic with range and damage
        Ray ray = new Ray(transform.position, targetPosition - transform.position);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, skill.range))
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                // Apply damage to the enemy
                hitInfo.collider.GetComponent<Enemy>().TakeDamage((int)skill.GetDamage(), skill.damageType);
            }
        }
    }

    void ApplyAreaOfEffectAttack(Vector3 targetPosition, SkillData skill)
    {
        // Implement area of effect logic with areaRadius and damage
        Collider[] hitColliders = Physics.OverlapSphere(targetPosition, skill.areaRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                // Apply damage to the enemy
                hitCollider.GetComponent<Enemy>().TakeDamage((int)skill.GetDamage(), skill.damageType);
            }
        }
    }

    void ApplyBuff(SkillData skill)
    {
        // Implement buff logic
        // Example: Increase player stats for a duration
        StartCoroutine(ApplyBuffCoroutine(skill));
    }

    IEnumerator ApplyBuffCoroutine(SkillData skill)
    {
        // Example buff effect: increase player's strength and agility
        int originalStrength = currentStrength;
        int originalAgility = currentAgility;

        currentStrength += (int)(originalStrength * 0.2f); // Increase strength by 20%
        currentAgility += (int)(originalAgility * 0.2f); // Increase agility by 20%

        yield return new WaitForSeconds(10f); // Buff duration

        // Revert back to original stats
        currentStrength = originalStrength;
        currentAgility = originalAgility;
    }

    void ApplyDebuff(Vector3 targetPosition, SkillData skill)
    {
        // Implement debuff logic
        Collider[] hitColliders = Physics.OverlapSphere(targetPosition, skill.areaRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                // Apply debuff to the enemy
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
