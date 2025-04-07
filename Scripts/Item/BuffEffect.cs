using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum StatType 
{
    strength, // 1 point increase damge by 1 and crit.power by 1%
    agility,  // 1 point increase evasion by 1% and crit.chance by 1%
    intelligence,   // 1 point increase magic damage by 1 and magic resistence by 3
    vitality,   // 1 point increase health by 3 or 5 points
    damage,
    critChance, 
    critPower,  //default value 150%
    maxHealth,
    armor,
    evasion,
    magicResistance,
    fireDamage,
    iceDamage,
    lightingDamage
}

[CreateAssetMenu(fileName = "New BuffEffect", menuName = "Data/Item/Buff Effect", order = 0)]
public class BuffEffect : ItemEffect 
{   
    private PlayerStats playerStats;
    [SerializeField] private StatType statType;
    [SerializeField] private int strength;
    [SerializeField] private float duartion;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
        playerStats.ModifyStatBy(GetStat(), strength, duartion);
    }

    private Stat GetStat()
    {
        switch(statType)
        {
            case StatType.agility:
                return playerStats.agility;

            case StatType.armor:
                return playerStats.armor;

            case StatType.critChance:
                return playerStats.critChance;

            case StatType.critPower:
                return playerStats.critPower;    

            case StatType.damage:
                return playerStats.damage;

            case StatType.evasion:
                return playerStats.evasion;

            case StatType.fireDamage:
                return playerStats.fireDamage;

            case StatType.iceDamage:
                return playerStats.iceDamage;

            case StatType.intelligence:
                return playerStats.intelligence;

            case StatType.lightingDamage:
                return playerStats.lightingDamage;

            case StatType.magicResistance:
                return playerStats.magicResistance;
                
            case StatType.maxHealth:
                return playerStats.maxHealth;
            
            case StatType.strength:
                return playerStats.strength;
            
            case StatType.vitality:
                return playerStats.vitality;
            
        }
        return null;
    }
}


