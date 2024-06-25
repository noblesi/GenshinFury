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
    [SerializeField] private string _name; // ������ �̸�
    [Multiline]
    [SerializeField] private string _tooltip; // ������ ����
    [SerializeField] private Sprite _iconSprite; // ������ ������
    [SerializeField] private GameObject _dropItemPrefab; // �ٴڿ� ������ �� ������ ������
    [SerializeField] private ItemType _itemType; // ������ ��з�
    [SerializeField] private ItemRarity _rarity; // ������ ���

    public abstract Item CreateItem();
}
