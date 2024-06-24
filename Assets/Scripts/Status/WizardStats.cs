using UnityEngine;

[CreateAssetMenu(fileName = "WizardStats", menuName = "PlayerClass/WizardStats")]
public class WizardStats : PlayerStats
{
    private void OnEnable()
    {
        initialHealth = 80;
        initialMana = 100;
        initialStrength = 10;
        initialAgility = 10;
        initialIntelligence = 25;

        healthPerLevel = 15;
        manaPerLevel = 25;
        strengthPerLevel = 2;
        agilityPerLevel = 2;
        intelligencePerLevel = 5;
    }
}
