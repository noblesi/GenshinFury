using UnityEngine;

public class Weapon : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 객체가 몬스터인지 확인
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Weapon hit the enemy!");
            // 몬스터에게 데미지를 주기 위해 IDamageable 인터페이스를 사용
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