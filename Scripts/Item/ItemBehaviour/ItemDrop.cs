using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{   
    [SerializeField] private int possibleItemDrop;
    [SerializeField] private ItemData[] possibleDrop;
    public List<ItemData> dropList = new List<ItemData>();
    
    [SerializeField] private GameObject dropPrefab;

    public virtual void GenerateDrop()
    {
        for(int i = 0; i < possibleDrop.Length; i++)
        {
            if(Random.Range(0,100) <= possibleDrop[i].DropChance)
            {
                dropList.Add(possibleDrop[i]);
            }
        }

        
        for(int i = 0; i < possibleItemDrop; i++)
        {   
            if(dropList.Count == 0)
                return;
            ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];
            dropList.Remove(randomItem);
            DropItem(randomItem);
        }
    }

    protected void DropItem(ItemData _itemData)
    {
        GameObject newDroppedItem = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 dropVelocity = new Vector2(Random.Range(-5f, 5f), Random.Range(18f,27f));

        newDroppedItem.GetComponent<ItemObject>().SetUpItem(_itemData, dropVelocity);
    }
}
