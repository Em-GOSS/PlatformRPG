using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
public enum UI_ItemSlotType
{
    UI_StashSlot,
    UI_InventorySlot,
    UI_Potion_ItemSlot,
    UI_EquipmentSlot,
    UI_CraftSlot,
    UI_FormulationSlot
}

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{   
    [Header("SlotType")]
    [SerializeField] private UI_ItemSlotType slotType;

    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText; 
    private RectTransform rectTransform;
    public InventoryItem inventoryItem;

    private void Start() 
    {
        rectTransform = this.GetComponent<RectTransform>();
    }
    public virtual void UpdateSlot(InventoryItem _inventoryItem)
    {   
        this.inventoryItem = _inventoryItem;
        itemImage.color = Color.white;
        if (inventoryItem != null)
        {
            itemImage.sprite = inventoryItem.data.icon;
            if (inventoryItem.stackSize > 1)
            {
                itemText.text = inventoryItem.stackSize.ToString();
            }
            else
            { 
                itemText.text = "";
            }
        }
    }

    public void ClearUpSlot()
    {
        inventoryItem = null;
        itemImage.color = Color.clear;
        itemImage.sprite = null;
        itemText.text = "";
    }


    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if(inventoryItem == null || inventoryItem.data == null)
            return;

        if(slotType == UI_ItemSlotType.UI_InventorySlot)
        {
            Inventory.Instance.EquipItem(inventoryItem.data);
        }
        else if(slotType == UI_ItemSlotType.UI_EquipmentSlot)
        {
            Inventory.Instance.UnloadItemByClick(inventoryItem.data);
        }           
        else if(slotType == UI_ItemSlotType.UI_Potion_ItemSlot)
        {   
            ItemData_Potion potionData = inventoryItem.data as ItemData_Potion;
            Inventory.Instance.CanUsePotion(potionData);
        }
        else if(slotType == UI_ItemSlotType.UI_CraftSlot)
        {
            ItemData_Equipment craftData = inventoryItem.data as ItemData_Equipment;
            Inventory.Instance.CanCraft(craftData, craftData.craftingMaterials);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(inventoryItem == null || inventoryItem.data == null)
            return;
        
        if(slotType == UI_ItemSlotType.UI_InventorySlot || slotType == UI_ItemSlotType.UI_EquipmentSlot)
        {
            ItemData_Equipment itemData_Equipment = inventoryItem.data as ItemData_Equipment;
            UIManager.Instance.CallOutToolTip(itemData_Equipment, new Vector3(rectTransform.position.x, rectTransform.position.y + rectTransform.pivot.y * rectTransform.rect.height));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.HideToolTip();
    }
}
