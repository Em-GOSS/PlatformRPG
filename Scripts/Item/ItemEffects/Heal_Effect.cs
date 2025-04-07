using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "Data/Item/Item Effects/Heal_Effect")]
public class Heal_Effect : ItemEffect
{   
    [Range(0, 1f)]
    [SerializeField] private float healPercentage; 

    public override void ExecuteEffect(Transform _enemyPosition)
    {   
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
        int healValue = Mathf.RoundToInt(healPercentage * playerStats.GetMaxHealthValue());
        playerStats.Heal(healValue);
    }
    

}
