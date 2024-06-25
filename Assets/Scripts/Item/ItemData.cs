using UnityEngine;
using static UnityEditor.Progress;

public abstract class ItemData : ScriptableObject
{
    public int ID => _id;
    public string Name => _name;
    public string Tooltip => _tooltip;
    public Sprite IconSprite => _iconSprite;
    public GameObject DropItemPrefab => _dropItemPrefab;
    public ItemType ItemType => _itemType;
    public ItemRarity Rarity => _rarity;

    [SerializeField] private int _id;
    [SerializeField] private string _name; // 아이템 이름
    [Multiline]
    [SerializeField] private string _tooltip; // 아이템 설명
    [SerializeField] private Sprite _iconSprite; // 아이템 아이콘
    [SerializeField] private GameObject _dropItemPrefab; // 바닥에 떨어질 때 생성할 프리팹
    [SerializeField] private ItemType _itemType; // 아이템 대분류
    [SerializeField] private ItemRarity _rarity; // 아이템 등급

    public abstract Item CreateItem();
}
