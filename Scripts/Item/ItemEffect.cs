using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemEffect", menuName = "Data/Item/Item Effect", order = 0)]
public class ItemEffect : ScriptableObject 
{
    public virtual void ExecuteEffect(Transform _enemyPosition)
    {
        Debug.Log("Execute Item Effect");
    }
}

