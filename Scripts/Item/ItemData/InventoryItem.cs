using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem 
{   
    public ItemData data;
    public int stackSize;
    public InventoryItem(ItemData _data)
    {
        this.data = _data;
        AddStack();
    }

    public void AddStack() => stackSize ++;

    public void RemoveStack() => stackSize --;

}
