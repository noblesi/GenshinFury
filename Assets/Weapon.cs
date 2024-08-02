using UnityEngine;

public class Weapon : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable enemy = other.GetComponent<IDamageable>();
            if (enemy != null)
            {
                int damage = GetComponentInParent<Player>().CalculateDamage();
                enemy.TakeDamage(damage);
            }
        }
    }
}