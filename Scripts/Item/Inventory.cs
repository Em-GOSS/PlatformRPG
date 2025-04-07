using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Inventory : SingletonMonoBehaviour<Inventory>, ISaveManager
{    
    public List<ItemData> startItems;

    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;
    public List<InventoryItem> stashItems;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    public List<InventoryItem> potionItems;
    public Dictionary<ItemData_Potion, InventoryItem> potionDictionary;
    public List<InventoryItem> equipments;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    [Header("InventoryUI")]
    [SerializeField] private Transform inventorySlotsParent;
    [SerializeField] private Transform stashSlotsParent;
    [SerializeField] private Transform potionSlotsParent;
    [SerializeField] private Transform equipmentSlotsParent;

    [SerializeField] UI_ItemSlot[] ui_Inventory_ItemSlots;
    [SerializeField] private int inventoryCount = 0;
    [SerializeField] UI_ItemSlot[] ui_Stash_ItemSlots;
    [SerializeField] private int stashCount = 0;
    [SerializeField] UI_ItemSlot[] ui_Potions_ItemSlots;
    [SerializeField] private int potionCount = 0;

    [SerializeField] UI_EquipmentSlot[] ui_EquipmentSlots;

    [Header("Potion CoolDown")]
    [SerializeField] private float lastTimeUsePotion = -99999f;



    [Header("Data Base")]
    public List<InventoryItem> loadedItems;
    public List<ItemData> itemDataBase;
    protected override void Start()
    {
        base.Start();

        inventoryItems = new List<InventoryItem>();
        stashItems = new List<InventoryItem>();
        potionItems = new List<InventoryItem>();
        equipments = new List<InventoryItem>();

        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();
        potionDictionary = new Dictionary<ItemData_Potion, InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();


        ui_Inventory_ItemSlots = inventorySlotsParent.GetComponentsInChildren<UI_ItemSlot>();
        ui_Stash_ItemSlots = stashSlotsParent.GetComponentsInChildren<UI_ItemSlot>();
        ui_Potions_ItemSlots = potionSlotsParent.GetComponentsInChildren<UI_ItemSlot>();
        ui_EquipmentSlots = equipmentSlotsParent.GetComponentsInChildren<UI_EquipmentSlot>();

        AddStartingItems();
    }

    private void AddStartingItems()
    {   
        if(loadedItems.Count > 0)
        {
            foreach(InventoryItem inventoryItem in loadedItems) 
            {
                for(int i = 0; i < inventoryItem.stackSize; i ++)
                {
                    AddItem(inventoryItem.data);
                }
            }
            return ;
        }

        for (int i = 0; i < startItems.Count; i++)
        {
            AddItem(startItems[i]);
        }
    }

    public void EquipItem(ItemData _item)
    {   
        ItemData_Equipment itemData_Equipment = _item as ItemData_Equipment;
        InventoryItem itemToEquip = new InventoryItem(itemData_Equipment);

        ItemData_Equipment itemToSwitch = null;
        foreach(KeyValuePair<ItemData_Equipment, InventoryItem> equipmentPair in equipmentDictionary)
        {
            if(equipmentPair.Key.equipmentType == itemData_Equipment.equipmentType)
            {
                itemToSwitch = equipmentPair.Key;
            }
        }

        if(itemToSwitch != null)
        {
            UnequipItem(itemToSwitch);
            AddItem(itemToSwitch);
        }

        equipments.Add(itemToEquip);
        equipmentDictionary.Add(itemData_Equipment, itemToEquip);
        itemData_Equipment.AddModifers();

        RemoveItem(_item);

        UI_EquipmentSlots_Update();

        UIManager.Instance.UpdateAllStatsUI();
    }
    public void UnloadItemByClick(ItemData _item)
    {   
        ItemData_Equipment itemData_Equipment = _item as ItemData_Equipment;
        InventoryItem itemToEquip = new InventoryItem(itemData_Equipment);

        ItemData_Equipment itemToUnload = null;
        foreach(KeyValuePair<ItemData_Equipment, InventoryItem> equipmentPair in equipmentDictionary)
        {
            if(equipmentPair.Key.equipmentType == itemData_Equipment.equipmentType)
            {
                itemToUnload = equipmentPair.Key;
            }
        }

        if(itemToUnload != null)
        {
            UnequipItem(itemToUnload);
            AddItem(itemToUnload);
        }
        
        UI_EquipmentSlots_Update();
        UIManager.Instance.UpdateAllStatsUI();
    }

    public void UnequipItem(ItemData_Equipment itemToUnequip)
    {
        if (equipmentDictionary.TryGetValue(itemToUnequip, out InventoryItem equipItemNow))
        {
            equipments.Remove(equipItemNow);
            equipmentDictionary.Remove(itemToUnequip);
        }
        itemToUnequip.RemvoveModifers();
    }

    public void UI_EquipmentSlots_Update()
    {
        for(int i = 0; i < ui_EquipmentSlots.Length; i++)
        {   
            ui_EquipmentSlots[i].ClearUpSlot();
            foreach(KeyValuePair<ItemData_Equipment, InventoryItem> equipmentPair in equipmentDictionary)
            {
                if(equipmentPair.Key.equipmentType == ui_EquipmentSlots[i].equipmentType)
                {
                    ui_EquipmentSlots[i].UpdateSlot(equipmentPair.Value);
                }
            }
        }
    }

    public void UI_ItemSlots_Update(ItemType _itemType)
    {      
        if(_itemType == ItemType.Equippment)
        {
            for(int i = 0; i < inventoryItems.Count; i++) 
            {           
                ui_Inventory_ItemSlots[i].UpdateSlot(inventoryItems[i]);
            }
            
            for(int i = inventoryItems.Count; i < inventoryCount && i < ui_Inventory_ItemSlots.Length; i++)
            {
                ui_Inventory_ItemSlots[i].ClearUpSlot();
            }
        }
        
        if(_itemType == ItemType.Material)
        {
            for(int i = 0; i< stashItems.Count; i++) 
            {           
                ui_Stash_ItemSlots[i].UpdateSlot(stashItems[i]);
            }
            
            for(int i = stashItems.Count; i < stashCount && i < ui_Stash_ItemSlots.Length; i++)
            {
                ui_Stash_ItemSlots[i].ClearUpSlot();
            }
        }

        if(_itemType == ItemType.Potion)
        {
            for(int i = 0; i < potionItems.Count; i++) 
            {           
                ui_Potions_ItemSlots[i].UpdateSlot(potionItems[i]);
            }
            
            for(int i = potionItems.Count; i < potionCount && i < ui_Potions_ItemSlots.Length; i++)
            {
                ui_Potions_ItemSlots[i].ClearUpSlot();
            }
        }
        
        
    }

    public void AddItem(ItemData _item)
    {
        if(_item.itemType == ItemType.Equippment)
        {
            AddToInventory(_item);
            UI_ItemSlots_Update(_item.itemType);
            inventoryCount++;
            
        }
        else if(_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
            UI_ItemSlots_Update(_item.itemType);
            stashCount++;
        }
        else if(_item.itemType == ItemType.Potion)
        {   
            ItemData_Potion itemData_Potion = _item as ItemData_Potion;
            AddToPotions(itemData_Potion);
            UI_ItemSlots_Update(_item.itemType);
            potionCount++;
        }

        
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem inventoryItem))
        {
            inventoryItem.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stashItems.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
        
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem inventoryItem))
        {
            inventoryItem.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventoryItems.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
        
    }

    private void AddToPotions(ItemData_Potion _itemDataPotion)
    {   
        if(potionDictionary.TryGetValue(_itemDataPotion, out InventoryItem inventoryItem))
        {
            inventoryItem.AddStack();
        }
        else
        {   
            InventoryItem newItem = new InventoryItem(_itemDataPotion);
            potionItems.Add(newItem);
            potionDictionary.Add(_itemDataPotion, newItem);
        }
    }
    public void RemoveItem(ItemData _item)
    {
        if(inventoryDictionary.TryGetValue(_item, out InventoryItem inventoryValue))
        {
            if(inventoryValue.stackSize <= 1)
            {   
                inventoryItems.Remove(inventoryValue);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                inventoryValue.RemoveStack();
            }
            UI_ItemSlots_Update(_item.itemType);
            inventoryCount--;
        }

        if(stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if(stashValue.stackSize <= 1)
            {
                stashItems.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
            {
                stashValue.RemoveStack();
            }
            UI_ItemSlots_Update(_item.itemType);
            stashCount--;
        }
    }
    public bool CanUsePotion(ItemData_Potion _potionToUse)
    {
        bool canUse = Time.time - lastTimeUsePotion > _potionToUse.potionCoolDown;
        UIManager.Instance.UpdateAllStatsUI();
        if(canUse)
        {
            _potionToUse.ProcessItemEffects(null);
            lastTimeUsePotion = Time.time;
            return true;
        }   
        else
        {
            Debug.Log("Potion is under cooling down");
            return false;
        }       
    }
    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();

        for(int i = 0; i < _requiredMaterials.Count; i++)
        {
            if(stashDictionary.TryGetValue(_requiredMaterials[i].data, out InventoryItem requiredItem))
            {
                if(requiredItem.stackSize < _requiredMaterials[i].stackSize)
                {
                    Debug.Log("not enough materials");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(_requiredMaterials[i]);
                }
            }
            else
            {
                Debug.Log("Do not has this material");
                return false;
            }
        }

        for(int i = 0; i < materialsToRemove.Count; i++)
        {   
            for(int j = 0; j < materialsToRemove[i].stackSize; j++)
            {
                RemoveItem(materialsToRemove[i].data);
            }
        }
        
        AddItem(_itemToCraft);
        Debug.Log("Here is your item" + _itemToCraft);

        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipments;
    public List<InventoryItem> GetStashList() => stashItems;
    public ItemData_Equipment GetEquipment(EquipmentType _equipmentType)
    {   
        ItemData_Equipment equipedItem = null;
        foreach(KeyValuePair<ItemData_Equipment, InventoryItem> equipmentPair in equipmentDictionary)
        {   
            if(equipmentPair.Key.equipmentType == _equipmentType)
            {
                equipedItem = equipmentPair.Key;
            }
        }
        return equipedItem;
    }

    
    public int GetStashItemCount(ItemData itemData)
    {
        InventoryItem targetInventoryItem;

        if(stashDictionary.TryGetValue(itemData, out targetInventoryItem)) 
        {   
            return targetInventoryItem.stackSize;
        }
        else
        {
            return 0;
        }

        
    }

    void ISaveManager.LoadData(GameData gameData)
    {
        foreach(KeyValuePair<string, int> pair in gameData.inventory)
        {
            foreach(var item in itemDataBase)
            {
                if(item != null && item.itemID == pair.Key)
                {
                    InventoryItem itemToLoad =  new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach(KeyValuePair<string, int> pair in gameData.stash)
        {
            foreach(var item in itemDataBase)
            {
                if(item != null && item.itemID == pair.Key)
                {
                    InventoryItem itemToLoad =  new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    loadedItems.Add(itemToLoad);
                }
            }
        }
    }

    void ISaveManager.SaveData(ref GameData gameData)
    {   
        gameData.inventory.Clear();
        gameData.stash.Clear();
        
        foreach(KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionary)
        {
            gameData.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
        }

        foreach(KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            gameData.stash.Add(pair.Key.itemID, pair.Value.stackSize);
        }
    }

#if UNITY_EDITOR

    [ContextMenu("Fill up itembase")]
    private void FillItemDataBase() => itemDataBase = new List<ItemData>(GetItemDataBase());
    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] {"Assets/Prefabs/ScriptableObject/ItemDatas"});

        foreach(string SOName in assetNames)
        { 
            var SOPath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOPath);
            itemDataBase.Add(itemData);
        }
        return itemDataBase;
    }
#endif

}
