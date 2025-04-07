using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_CraftPanel : MonoBehaviour
{    
    private ItemData_Equipment currentItemData;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemBouns;

    [SerializeField] private GameObject formulationSlotPrefab; 
    [SerializeField] private Transform formulationSlotsParent;
   
    private List<UI_FormulationSlot> formulationSlots = new List<UI_FormulationSlot>();
   
    public void SetUpCraftPanel(ItemData itemData)
    {   
        ItemData_Equipment itemData_Equipment = itemData as ItemData_Equipment;
        UpdateMaterialPanel(itemData_Equipment);
        SetCurrentItemData(itemData);
        itemImage.sprite = itemData.icon;
        itemName.text = itemData.itemName;
        itemBouns.text = itemData.GetBouns();
        
    }

    public void UpdateMaterialPanel(ItemData_Equipment itemData_Equipment)
    {
        foreach (UI_FormulationSlot originInventoryItem in formulationSlots)
        {
            Destroy(originInventoryItem.gameObject);
        }
        formulationSlots.Clear();

        foreach (InventoryItem inventoryItem_Craft in itemData_Equipment.craftingMaterials)
        {
            GameObject newFormulationSlot = Instantiate(formulationSlotPrefab, formulationSlotsParent);
            UI_FormulationSlot slotController = newFormulationSlot.GetComponent<UI_FormulationSlot>();
            slotController.UpdateSlot(inventoryItem_Craft);
            formulationSlots.Add(slotController);
        }
    }

    public void SetCurrentItemData(ItemData itemData)
    {   
        ItemData_Equipment itemData_Equipment = itemData as ItemData_Equipment;
        currentItemData = itemData_Equipment;
    }


    public void TryToCraft() 
    {
        Inventory.Instance.CanCraft(currentItemData, currentItemData.craftingMaterials);
        UpdateMaterialPanel(currentItemData);
    }
}
