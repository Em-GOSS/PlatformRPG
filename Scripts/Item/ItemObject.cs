using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{   
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;

    private void OnValidate()
    {
        if (itemData == null)
            return;
        SetUpVirtuals();
    }

    private void SetUpVirtuals()
    {
        this.GetComponent<SpriteRenderer>().sprite = itemData.icon;
        this.name = "Item Object - " + itemData.itemName;
    }

    public void SetUpItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;
        SetUpVirtuals();

    }

    public void PickUpItem()
    {
        Inventory.Instance.AddItem(itemData);
        Destroy(this.gameObject);
    }
}
