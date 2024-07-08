using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stats", menuName = "Player/Player Stats")]
public class PlayerStats : ScriptableObject
{
    public int maxHealth;
    public int currentHealth;
    public int maxMana;
    public int currentMana;
    public int strength;
    public int dexterity;
    public int intelligence;
    public int skillPoints;
    public int playerLevel;
}
