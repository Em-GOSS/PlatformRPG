using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatUnit : MonoBehaviour
{   
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI valueText; 
    
    private void OnEnable() 
    {
        UpdateStat();
    }
    public void UpdateStat()
    {
        valueText.text = GetStat()?.GetValue().ToString(); 
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
