using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject_Trigger : MonoBehaviour
{
    private ItemObject itemObject => GetComponentInParent<ItemObject>();

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.GetComponent<Player>() != null)
        {                               
            if(collision.GetComponent<Entity>().isDead)
                return;
            Debug.Log("Picked Up Item!");                                             
            itemObject.PickUpItem();
        }
    }
}
