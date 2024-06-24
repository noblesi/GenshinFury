using UnityEngine;

[CreateAssetMenu(fileName = "WarriorStats", menuName = "PlayerClass/WarriorStats")]
public class WarriorStats : PlayerStats
{
    private void OnEnable()
    {
        initialHealth = 150;
        initialMana = 50;
        initialStrength = 20;
        initialAgility = 10;
        initialIntelligence = 5;

        healthPerLevel = 30;
        manaPerLevel = 10;
        strengthPerLevel = 5;
        agilityPerLevel = 2;
        intelligencePerLevel = 1;
    }
}
