using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAndFire_Controller : MonoBehaviour
{
     private PlayerStats playerStats;

    private void Start()
    {
        playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
    }

    
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.GetComponent<Enemy>() != null)
        {   
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
            playerStats.DoMagicalDamage(enemyStats);
        }
    }
}
