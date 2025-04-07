using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_FormulationSlot : UI_ItemSlot
{   
    [SerializeField] Color insufficientColor;
    [SerializeField] Color sufficientColor;
    public override void UpdateSlot(InventoryItem _inventoryItem)
    {
        this.inventoryItem = _inventoryItem;
        itemImage.color = Color.white;
        int itemCapacity = Inventory.Instance.GetStashItemCount(inventoryItem.data);
        if (inventoryItem != null)
        {
            itemImage.sprite = inventoryItem.data.icon;
            itemText.text = inventoryItem.stackSize.ToString() +" / " + itemCapacity.ToString();

            if(itemCapacity < inventoryItem.stackSize) 
                itemText.color = insufficientColor;
            else 
                itemText.color = sufficientColor;
        }

    }

}
