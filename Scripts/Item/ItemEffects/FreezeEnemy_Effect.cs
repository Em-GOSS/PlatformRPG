using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "FreezeEnemy", menuName = "Data/Item/Item Effects/FreezeEnemy_Effect")]
public class FreezeEnemy_Effect : ItemEffect
{   
    [SerializeField] private float freezeDuartion;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(_enemyPosition.position, 2);

        foreach(Collider2D hit in hits) 
        {
           hit.GetComponent<Enemy>()?.FreezeTimeFor(freezeDuartion);
        }
    }
}
