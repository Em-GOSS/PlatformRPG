using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTrigger : MonoBehaviour
{
    private Enemy enemy;
    private void Awake()
    {
        enemy=this.GetComponentInParent<Enemy>();
    }

    private void AnimationTrigger() 
    {
        enemy.AnimationTrigger();
    }

    private void DamageCauseTrigger() 
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(enemy.attack_Check.position,enemy.attack_CheckRadius);

        foreach(Collider2D hit in hits) 
        {
            if(hit.GetComponent<Player>()!=null)
            {   
                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(target);
            }
        }
    }

    private void OpenCounterAttackWindow() => enemy.OpenCounterAttackWindow();

    private void CloseCounterAttackWindow() => enemy.CloseCounterAttackWindow();

}
