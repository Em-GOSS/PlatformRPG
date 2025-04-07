using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot, IPointerClickHandler
{      
    public void SetUpCraftSlot(ItemData itemData)
    {   
        this.inventoryItem.data = itemData;
        this.itemImage.sprite = itemData.icon;
        this.itemText.text = itemData.itemName;
    }   

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance.UpdateCraftPanel(this.inventoryItem.data);
    }

}
