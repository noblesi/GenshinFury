using UnityEngine;
using System.Collections.Generic;

public class PlayerSkills : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private List<BasePlayerClass> playerClasses;
    [SerializeField] private int currentClassIndex = 0;

    private BasePlayerClass currentClass;
    private Dictionary<KeyCode, float> skillCooldownTimers = new Dictionary<KeyCode, float>();

    void Start()
    {
        SetCurrentClass(currentClassIndex);
    }

    void Update()
    {
        UpdateCooldownTimers();
    }

    public void SetCurrentClass(int classIndex)
    {
        if (classIndex >= 0 && classIndex < playerClasses.Count)
        {
            currentClassIndex = classIndex;
            currentClass = playerClasses[currentClassIndex];
            InitializeCooldownTimers();
        }
    }

    void InitializeCooldownTimers()
    {
        skillCooldownTimers.Clear();
        foreach (var skill in currentClass.skillBook)
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

    public void HandleSkillInput()
    {
        foreach (var skill in currentClass.skillBook)
        {
            if (Input.GetKeyDown(skill.key) && skillCooldownTimers[skill.key] <= 0)
            {
                UseSkill(skill);
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

        // Implement skill-specific logic here, such as damage, heal, buff, debuff, etc.
        ApplySkillEffects(skill);
    }

    void ApplySkillEffects(SkillData skill)
    {
        float damage = skill.GetDamage();

        // Add your skill effect logic here based on skill types and damage type
        foreach (var type in skill.types)
        {
            switch (type)
            {
                case SkillType.Melee:
                    // Implement melee attack logic with damage
                    break;
                case SkillType.Ranged:
                    // Implement ranged attack logic with damage
                    break;
                case SkillType.SingleTarget:
                    // Implement single target logic with damage
                    break;
                case SkillType.AreaOfEffect:
                    // Implement area of effect logic with damage
                    break;
                case SkillType.Buff:
                    // Implement buff logic
                    break;
                case SkillType.Debuff:
                    // Implement debuff logic
                    break;
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
}
