using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    private bool isDebuffed;
    private float debuffDuration;
    private float debuffStartTime;

    public EquipmentDatabase equipmentDatabase; // Equipment database reference

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, DamageType damageType)
    {
        currentHealth -= damage;
        Debug.Log($"Enemy took {damage} {damageType} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ApplyDebuff(SkillData debuffSkill)
    {
        if (!isDebuffed)
        {
            StartCoroutine(ApplyDebuffCoroutine(debuffSkill));
        }
    }

    private IEnumerator ApplyDebuffCoroutine(SkillData debuffSkill)
    {
        isDebuffed = true;
        debuffDuration = debuffSkill.GetCooldown(); // assuming debuff duration is stored in cooldowns

        // Example debuff: decrease enemy stats
        // You can add different types of debuffs here based on your game design
        float originalSpeed = GetComponent<NavMeshAgent>().speed;
        GetComponent<NavMeshAgent>().speed *= 0.5f; // Decrease speed by 50%

        yield return new WaitForSeconds(debuffDuration);

        // Revert back to original stats
        GetComponent<NavMeshAgent>().speed = originalSpeed;
        isDebuffed = false;
    }

    private void Die()
    {
        Debug.Log("Enemy died.");
        DropLoot();
        Destroy(gameObject);
    }

    private void DropLoot()
    {
        // Check if an item should drop
        if (Random.value <= equipmentDatabase.itemDropChance)
        {
            // Example: Drop a random equipment item
            EquipmentItem droppedItem = equipmentDatabase.GetRandomEquipment(EquipmentType.Weapon, WeaponType.Sword);
            if (droppedItem != null)
            {
                Debug.Log($"Dropped item: {droppedItem.itemName} ({droppedItem.rarity})");
                // Implement logic to instantiate and show the dropped item in the game world
            }
        }
        else
        {
            Debug.Log("No item dropped.");
        }
    }
}
