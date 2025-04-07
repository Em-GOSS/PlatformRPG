using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftManager : MonoBehaviour
{
    [SerializeField] private List<ItemData_Equipment> craftableItems; 
    private List<ItemData_Equipment> currentCraftList = new List<ItemData_Equipment>();

    [SerializeField] private GameObject craftSlotPrefab;
    [SerializeField] private Transform craftSlotsParent;
    private List<UI_CraftSlot> craftSlots = new List<UI_CraftSlot>();

    [SerializeField] private UI_CraftPanel craftPanel;
    
    private void Start()
    {
        UseFilter((int)EquipmentType.All);
    }
    ///
    // void RegisterNewFormulation()
    // {

    // }
    /// 


    public void InitializeCraftUI()
    {
        UseFilter((int)EquipmentType.All);
        craftPanel.SetUpCraftPanel(currentCraftList[0]);
    }


    public void UseFilter(int filterType)
    {   
        BuildCraftableList((EquipmentType)filterType);
        UpdateCraftList();
    }

    public void BuildCraftableList(EquipmentType equipmentType_Filter) 
    {   
        currentCraftList.Clear();
        if(equipmentType_Filter != EquipmentType.All)
        {
            foreach(ItemData_Equipment craftData in craftableItems)
            {
                if(craftData.equipmentType == equipmentType_Filter)
                {
                    currentCraftList.Add(craftData);
                }
            }
        }
        else
        {
            foreach(ItemData_Equipment craftData in craftableItems)
            {
                currentCraftList.Add(craftData);
            }
        }
        
    }
    
    private void UpdateCraftList()
    {   
        foreach(UI_CraftSlot craftSlot in craftSlots)
        {   
            Destroy(craftSlot.gameObject);
        }
        
        craftSlots.Clear();

        foreach(ItemData_Equipment itemToCraft in currentCraftList)
        {
            GameObject newCraftSlot = Instantiate(craftSlotPrefab, craftSlotsParent);
            UI_CraftSlot ui_CraftSlot = newCraftSlot.GetComponent<UI_CraftSlot>();
            ui_CraftSlot.SetUpCraftSlot(itemToCraft);

            craftSlots.Add(ui_CraftSlot);
        }
    }


    public void UpdateCraftPanel(ItemData itemData)
    {
        craftPanel.SetUpCraftPanel(itemData);
    }

}
