using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion Item Data", menuName = "Data/Item/Item_Potion", order = 0)]
public class ItemData_Potion : ItemData
{   
    public float potionCoolDown;
    public ItemEffect[] itemEffects;
    public void ProcessItemEffects(Transform _enemyPosition)
    {   
        foreach(ItemEffect itemEffect in itemEffects)
        {   
            itemEffect.ExecuteEffect(_enemyPosition);
        }
    }
}
