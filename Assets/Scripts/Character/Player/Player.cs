using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : BaseCharacter
{
    [SerializeField] private PlayerSkillManager skillManager;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private InventoryHolder inventoryHolder;
    protected List<SkillData> playerSkills = new List<SkillData>();

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
    }

    public virtual void Initialize(PlayerData playerData)
    {
        this.playerData = playerData;
        playerData.PlayerStats.currentHealth = playerData.PlayerStats.maxHealth;
        playerData.PlayerStats.currentHealth = playerData.PlayerStats.maxMana;

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
}

