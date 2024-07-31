using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Player/Player Data")]
public class PlayerData : ScriptableObject
{
    public int maxHealth;
    public int currentHealth;
    public int maxMana;
    public int currentMana;
    public int strength;
    public int dexterity;
    public int intelligence;
}
