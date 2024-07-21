using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseCharacter
{
    [SerializeField] private PlayerSkillManager skillManager;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] private Animator animator;
    protected List<SkillData> playerSkills = new List<SkillData>();
    public float attackCooldown = 1f;
    private float lastAttackTime;

    public PlayerSkillManager SkillManager => skillManager;
    public PlayerData PlayerData => playerData;

    protected override void Start()
    {
        base.Start();

        if (skillManager == null)
        {
            skillManager = gameObject.AddComponent<PlayerSkillManager>();
        }

        inventoryHolder = GetComponent<InventoryHolder>();
        EnsureComponents();

        // Animator가 null인지 확인
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError($"{gameObject.name}에 Animator 컴포넌트가 없습니다.");
            }
        }
    }

    private void EnsureComponents()
    {
        EnsureComponent<Collider>(gameObject);
        EnsureRigidbody(gameObject);
    }

    public virtual void Initialize(PlayerData playerData)
    {
        this.playerData = playerData;
        maxHealth = playerData.PlayerStats.maxHealth;
        currentHealth = playerData.PlayerStats.maxHealth;
        maxMana = playerData.PlayerStats.maxMana;
        currentMana = playerData.PlayerStats.maxMana;

        skillManager.SetPlayerStats(playerData.PlayerStats);
        playerSkills = LoadSkillsFromLibraries(playerData.SkillLibrary);
        skillManager.SetSkills(playerSkills);
    }

    protected List<SkillData> LoadSkillsFromLibraries(List<SkillLibrary> skillLibraries)
    {
        List<SkillData> skills = new List<SkillData>();
        foreach (var library in skillLibraries)
        {
            skills.AddRange(library.PlayerSkillLibrary);
        }
        return skills;
    }

    private void Update()
    {
        HandleSkillInput();
        HandleAttackInput();
    }

    protected void HandleSkillInput()
    {
        foreach (var skill in playerSkills)
        {
            if (Input.GetKeyDown(skill.Key) && SkillManager.GetSkillCooldown(skill.Key) <= 0)
            {
                UseSkill(skill);
                SkillManager.TriggerSkillCooldown(skill);
            }
        }
    }

    protected void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            PerformAttack();
            lastAttackTime = Time.time;
        }
    }

    public void UseSkill(SkillData skill)
    {
        if (skillManager.IsSkillUnlocked(skill) && currentMana >= skill.GetManaCost())
        {
            ChangeMana(currentMana - skill.GetManaCost());
            Debug.Log($"{gameObject.name} used skill: {skill.SkillName}");
        }
    }

    public void SkillLevelUp(SkillData skill)
    {
        skill.SkillLevelUp();
    }

    protected override void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
    }

    public void ApplyBuff(ConsumableItemData consumableItem)
    {
        StartCoroutine(ApplyBuffCoroutine(consumableItem));
    }

    protected IEnumerator ApplyBuffCoroutine(ConsumableItemData consumableItem)
    {
        int originalStrength = playerData.PlayerStats.strength;
        int originalDexterity = playerData.PlayerStats.dexterity;

        playerData.PlayerStats.strength += (int)(originalStrength * 0.2f);
        playerData.PlayerStats.dexterity += (int)(originalDexterity * 0.2f);

        yield return new WaitForSeconds(consumableItem.buffDuration);

        playerData.PlayerStats.strength = originalStrength;
        playerData.PlayerStats.dexterity = originalDexterity;
    }

    public void PickUpItem(InventoryItemData itemData, int amount)
    {
        if (inventoryHolder.InventorySystem.AddToInventory(itemData, amount))
        {
            Debug.Log($"Picked up {amount} of {itemData.name}");
        }
        else
        {
            Debug.Log("Inventory is full");
        }
    }

    private void PerformAttack()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                int damage = CalculateDamage();
                enemy.TakeDamage(damage, DamageType.Physical);
                Debug.Log($"{gameObject.name} attacked {enemy.gameObject.name} and dealt {damage} damage. Enemy health: {enemy.currentHealth}");

                // 공격 애니메이션 트리거
                if (animator != null)
                {
                    animator.SetTrigger("Attack");
                }
            }
            else
            {
                Debug.Log($"{gameObject.name} missed the attack.");
            }
        }
    }

    public int CalculateDamage()
    {
        return (int)(playerData.PlayerStats.strength * 2) +
               (int)(playerData.PlayerStats.dexterity * 1.5f) +
               (int)(playerData.PlayerStats.intelligence * 1.2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(other.GetComponent<Enemy>().attackDamage, DamageType.Physical);
        }
    }
}
