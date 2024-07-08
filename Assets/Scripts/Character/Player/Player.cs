using System.Collections.Generic;
using UnityEngine;

public class Player : BaseCharacter
{
    [SerializeField] private PlayerSkillManager skillManager;
    [SerializeField] private PlayerData playerData;
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
}
