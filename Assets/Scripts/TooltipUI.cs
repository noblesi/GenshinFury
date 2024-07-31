using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour
{
    public Text itemName;
    public Text itemDescription;
    public GameObject tooltipPanel;

    private void Start()
    {
        HideTooltip();
    }

    public void ShowTooltip(Item item)
    {
        itemName.text = item.itemName;
        itemDescription.text = item.description;
        tooltipPanel.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}
