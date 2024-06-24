using UnityEngine;

[CreateAssetMenu(fileName = "ArcherStats", menuName = "PlayerClass/ArcherStats")]
public class ArcherStats : PlayerStats
{
    private void OnEnable()
    {
        initialHealth = 100;
        initialMana = 60;
        initialStrength = 15;
        initialAgility = 20;
        initialIntelligence = 10;

        healthPerLevel = 20;
        manaPerLevel = 15;
        strengthPerLevel = 3;
        agilityPerLevel = 5;
        intelligencePerLevel = 2;
    }
}
