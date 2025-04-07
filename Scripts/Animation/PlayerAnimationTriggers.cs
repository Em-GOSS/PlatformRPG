using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player;
    
    private void Awake() 
    {
        player = this.GetComponentInParent<Player>();
    }

    private void AnimationTrigger() 
    {
        player.AnimationTrigger();
    }

    private void DamageCauseTrigger() 
    {   
        Collider2D[] hits = Physics2D.OverlapCircleAll(player.attack_Check.position,player.attack_CheckRadius);

        foreach(Collider2D hit in hits) 
        {
            if(hit.GetComponent<Enemy>()!=null)
            {       
                EnemyStats _targetStats = hit.GetComponent<EnemyStats>();
                player.stats.DoDamage(_targetStats);
                //inventory get weapon call item effect
                Inventory.Instance.GetEquipment(EquipmentType.Weapon)?.ProcessItemEffects(_targetStats.transform);        
            }
        }
    }

    #region Skills

    private void ThrowSwordTrigger()
    {
        SkillManager.Instance.sword_Skill.CreateSword();
    }

    #endregion

}
