using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_ToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private TextMeshProUGUI itemBouns;

    public void SetToolTip(ItemData_Equipment itemData_Equipment)
    {
        itemName.text = itemData_Equipment.itemName;
        itemDescription.text = itemData_Equipment.itemType.ToString();
        itemBouns.text = itemData_Equipment.GetBouns();
    }

    public void ActiveToolTip(ItemData_Equipment itemData_Equipment)
    {      
        this.gameObject.SetActive(true);
        SetToolTip(itemData_Equipment);
        
    }

    public void HideToolTip()
    {
        this.gameObject.SetActive(false);
    }
}
