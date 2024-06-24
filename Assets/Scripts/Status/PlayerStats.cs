using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "PlayerClass/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public int initialHealth;
    public int initialMana;
    public int initialStrength;
    public int initialAgility;
    public int initialIntelligence;

    public int healthPerLevel;
    public int manaPerLevel;
    public int strengthPerLevel;
    public int agilityPerLevel;
    public int intelligencePerLevel;
}
