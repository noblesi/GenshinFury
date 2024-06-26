using UnityEngine;

[CreateAssetMenu(fileName = "New Drop Item", menuName = "Items/DropItem")]
public class DropItemData : ScriptableObject
{
    public int ID => _id;
    public string Name => _name;
    public DropItemType DropItemType => _dropItemType;
    public Sprite IconSprite => _iconSprite;
    public GameObject DropItemPrefab => _dropItemPrefab;

    [SerializeField] private int _id;
    [SerializeField] private string _name; // 아이템 이름
    [SerializeField] private DropItemType _dropItemType; // 드랍 아이템 유형
    [SerializeField] private Sprite _iconSprite; // 아이템 아이콘
    [SerializeField] private GameObject _dropItemPrefab; // 드랍 아이템 프리팹
}
