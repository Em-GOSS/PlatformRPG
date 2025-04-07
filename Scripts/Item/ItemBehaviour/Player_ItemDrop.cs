using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ItemDrop : ItemDrop
{
   [Header("Player's drop")]
   [SerializeField] private float chanceToLoseItems;
   [SerializeField] private float chanceToLoseMaterials;

    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.Instance;

        List<InventoryItem> stashItems = inventory.GetStashList();
        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        List<InventoryItem> stashItemsToRemove = new List<InventoryItem>();


        foreach(InventoryItem inventoryItem in inventory.GetEquipmentList())
        {
            if(Random.Range(0,100) <= chanceToLoseItems)
            {
                itemsToUnequip.Add(inventoryItem);
            }
        }

        foreach(InventoryItem inventoryItem in inventory.GetStashList())
        {
            if(Random.Range(0,100) <= chanceToLoseMaterials)
            {
                stashItemsToRemove.Add(inventoryItem);
            }
        }


        foreach(InventoryItem ItemToUnequip in itemsToUnequip)
        {
            inventory.UnequipItem(ItemToUnequip.data as ItemData_Equipment);
            DropItem(ItemToUnequip.data);
        }

        foreach(InventoryItem ItemToRemove in stashItemsToRemove)
        {
            inventory.RemoveItem(ItemToRemove.data);
            DropItem(ItemToRemove.data);
        }
        
    }


}
