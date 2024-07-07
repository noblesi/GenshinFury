using UnityEngine;

public class QuickSlotSystemHolder : MonoBehaviour
{
    [SerializeField] private int quickSlotSize;
    [SerializeField] private QuickSlotSystem quickSlotSystem;

    public QuickSlotSystem QuickSlotSystem => quickSlotSystem;

    private void Awake()
    {
        quickSlotSystem = new QuickSlotSystem(quickSlotSize);
    }
}
