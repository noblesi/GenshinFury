using UnityEngine;

[CreateAssetMenu(fileName = "MeteorShower", menuName = "Skill/MeteorShower")]
public class MeteorShower : SkillData
{
    private void OnEnable()
    {
        skillName = "Meteor Shower";
        types = new SkillType[] { SkillType.Ranged, SkillType.AreaOfEffect };
        damageType = DamageType.Magical;
        baseDamage = 2.0f;
        damagePerLevel = 0.5f;
        cooldowns = new float[] { 60.0f, 55.0f, 50.0f };
        effect = null;  // Assign appropriate ParticleSystem
        animationTrigger = "Skill4";
        key = KeyCode.R;
        level = 1;
        maxLevel = 3;
        evolvedSkill = null;
    }
}
