using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseCharacter
{
    [SerializeField] private Animator animator;

    protected BasePlayerClass currentClass;
    protected Dictionary<KeyCode, float> skillCooldownTimers = new Dictionary<KeyCode, float>();

    // Current stats
    protected int baseStrength;
    protected int baseAgility;
    protected int baseIntelligence;
    protected int currentStrength;
    protected int currentAgility;
    protected int currentIntelligence;
    protected int currentLevel = 1;

    public int skillPoints = 0; // 스킬 포인트

    // Equipment slots
    public EquipmentItem helmet;
    public EquipmentItem armor;
    public EquipmentItem gloves;
    public EquipmentItem boots;
    public EquipmentItem weapon;

    // Quick slots
    public ItemData[] quickItemSlots = new ItemData[4];
    public SkillData[] quickSkillSlots = new SkillData[4];

    // Skill books
    public List<SkillData> commonSkills = new List<SkillData>();

    private const float BuffDuration = 10f;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    public int MaxMana => maxMana;
    public int CurrentMana => currentMana;

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
        HandleItemInput();
    }

    protected void InitializeCooldownTimers()
    {
        skillCooldownTimers.Clear();
        foreach (var skill in quickSkillSlots)
        {
            if (skill != null)
            {
                skillCooldownTimers[skill.key] = 0f;
            }
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
        maxMana = currentClass.stats.initialMana;
        baseStrength = currentClass.stats.initialStrength;
        baseAgility = currentClass.stats.initialAgility;
        baseIntelligence = currentClass.stats.initialIntelligence;

        UpdateStats();
    }

    protected void UpdateStats()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
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
        for (int i = 0; i < quickSkillSlots.Length; i++)
        {
            var skill = quickSkillSlots[i];
            if (skill != null && Input.GetKeyDown(skill.key) && skillCooldownTimers[skill.key] <= 0)
            {
                UseSkill(skill);
            }
        }
    }

    public virtual void HandleItemInput()
    {
        for (int i = 0; i < quickItemSlots.Length; i++)
        {
            var item = quickItemSlots[i];
            if (item != null && Input.GetKeyDown((KeyCode)item.ID))
            {
                UseItem(item);
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
            ChangeMana(currentMana - (int)skill.GetManaCost()); // Mana 변경 및 이벤트 호출
        }
    }

    protected void UseItem(ItemData item)
    {
        // 아이템 사용 로직
        Debug.Log($"Used item: {item.Name}");
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
        SkillData skill = commonSkills.Find(s => s.skillName == skillName);
        if (skill != null && skillPoints > 0 && skill.level < skill.maxLevel)
        {
            skill.LevelUp();
            skillPoints--;
            OnSkillsChanged?.Invoke();
            return true;
        }
        return false;
    }

    public void AddSkillPoint()
    {
        skillPoints++;
    }

    public void LevelUp()
    {
        currentLevel++;
        currentHealth = currentClass.stats.initialHealth;
        currentMana = currentClass.stats.initialMana;
        currentStrength = currentClass.stats.initialStrength;
        currentAgility = currentClass.stats.initialAgility;
        currentIntelligence = currentClass.stats.initialIntelligence;
        AddSkillPoint(); // 레벨업 시 스킬 포인트 추가
    }

    public void SwapQuickSlots(int slot1, int slot2, QuickSlotType slotType)
    {
        if (slotType == QuickSlotType.Item)
        {
            ItemData temp = quickItemSlots[slot1];
            quickItemSlots[slot1] = quickItemSlots[slot2];
            quickItemSlots[slot2] = temp;
        }
        else if (slotType == QuickSlotType.Skill)
        {
            SkillData temp = quickSkillSlots[slot1];
            quickSkillSlots[slot1] = quickSkillSlots[slot2];
            quickSkillSlots[slot2] = temp;
        }
        OnSkillsChanged?.Invoke();
    }

    public event System.Action OnSkillsChanged;
}
