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
    [SerializeField] private string _name; // ������ �̸�
    [SerializeField] private DropItemType _dropItemType; // ��� ������ ����
    [SerializeField] private Sprite _iconSprite; // ������ ������
    [SerializeField] private GameObject _dropItemPrefab; // ��� ������ ������
}
