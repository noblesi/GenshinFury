using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public string description;
    public int maxStackSize = 1;
    public GameObject itemPrefab; // 아이템 프리팹 필드 추가
}
