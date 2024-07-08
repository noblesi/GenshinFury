using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Player/Player Data")]
public class PlayerData : ScriptableObject
{
    public PlayerStats PlayerStats;
    public List<SkillLibrary> SkillLibrary;
}
