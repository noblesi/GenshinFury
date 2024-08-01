using UnityEngine;

public class Weapon : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ��ü�� �������� Ȯ��
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Weapon hit the enemy!");
            // ���Ϳ��� �������� �ֱ� ���� IDamageable �������̽��� ���
            IDamageable enemy = other.GetComponent<IDamageable>();
            if (enemy != null)
            {
                int damage = GetComponentInParent<Player>().CalculateDamage();
                Debug.Log($"Dealing {damage} damage to {other.gameObject.name}");
                enemy.TakeDamage(damage);
            }
        }
    }
}