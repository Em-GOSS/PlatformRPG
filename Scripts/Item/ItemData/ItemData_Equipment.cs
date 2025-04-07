using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EquipmentType 
{
    Weapon,
    Armor,
    Amulet,
    Flask,
    All

}

[CreateAssetMenu(fileName = "New Equipment Item Data", menuName = "Data/Item/Item_Equipment", order = 0)]
public class ItemData_Equipment : ItemData
{   
    public EquipmentType equipmentType;

    public ItemEffect[] itemEffects;

    [Header ("Major Stats")]
    public int strength_Modifer; // 1 point increase damge by 1 and crit.power by 1%
    public int agility_Modifer;  // 1 point increase evasion by 1% and crit.chance by 1%
    public int intelligence_Modifer;   // 1 point increase magic damage by 1 and magic resistence by 3
    public int vitality_Modifer;   // 1 point increase health by 3 or 5 points

    [Header("Offsensive Stats")]
    public int damage_Modifer;
    public int critChance_Modifer; 
    public int critPower_Modifer;  //default value 150%

    [Header("Defensive Stats")]
    public int maxHealth_Modifer;
    public int armor_Modifer;
    public int evasion_Modifer;
    public int magicResistance_Modifer;

    [Header("Magic Stat")]
    public int fireDamage_Modifer;
    public int iceDamage_Modifer;
    public int lightingDamage_Modifer;

    


    public void AddModifers() 
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifer(strength_Modifer);
        playerStats.agility.AddModifer(agility_Modifer);
        playerStats.intelligence.AddModifer(intelligence_Modifer);
        playerStats.vitality.AddModifer(vitality_Modifer);

        playerStats.damage.AddModifer(damage_Modifer);
        playerStats.critChance.AddModifer(critChance_Modifer);
        playerStats.critPower.AddModifer(critPower_Modifer);
        
        playerStats.maxHealth.AddModifer(maxHealth_Modifer);
        playerStats.armor.AddModifer(armor_Modifer);
        playerStats.evasion.AddModifer(evasion_Modifer);
        playerStats.magicResistance.AddModifer(magicResistance_Modifer);

        playerStats.fireDamage.AddModifer(fireDamage_Modifer);
        playerStats.iceDamage.AddModifer(iceDamage_Modifer);
        playerStats.lightingDamage.AddModifer(lightingDamage_Modifer);

    }

    public void RemvoveModifers()
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifer(strength_Modifer);
        playerStats.agility.RemoveModifer(agility_Modifer);
        playerStats.intelligence.RemoveModifer(intelligence_Modifer);
        playerStats.vitality.RemoveModifer(vitality_Modifer);

        playerStats.damage.RemoveModifer(damage_Modifer);
        playerStats.critChance.RemoveModifer(critChance_Modifer);
        playerStats.critPower.RemoveModifer(critPower_Modifer);
        
        playerStats.maxHealth.RemoveModifer(maxHealth_Modifer);
        playerStats.armor.RemoveModifer(armor_Modifer);
        playerStats.evasion.RemoveModifer(evasion_Modifer);
        playerStats.magicResistance.RemoveModifer(magicResistance_Modifer);

        playerStats.fireDamage.RemoveModifer(fireDamage_Modifer);
        playerStats.iceDamage.RemoveModifer(iceDamage_Modifer);
        playerStats.lightingDamage.RemoveModifer(lightingDamage_Modifer);
    }


    public void ProcessItemEffects(Transform _enemyPosition)
    {   
        foreach(ItemEffect itemEffect in itemEffects)
        {   
            itemEffect.ExecuteEffect(_enemyPosition);
        }
    }


    public override string GetBouns()
    {   
        stringBuilder.Clear();
        AddItemBouns(strength_Modifer, "STR");
        AddItemBouns(agility_Modifer, "AGI");
        AddItemBouns(intelligence_Modifer, "INT");
        AddItemBouns(vitality_Modifer, "VIT");

        AddItemBouns(damage_Modifer,"DAM");
        AddItemBouns(critChance_Modifer, "CRI_Chance");
        AddItemBouns(critPower_Modifer, "CRI_Power");

        AddItemBouns(maxHealth_Modifer, "MaxHealth");
        AddItemBouns(armor_Modifer,"ARM");
        AddItemBouns(evasion_Modifer,"EVA");
        AddItemBouns(magicResistance_Modifer, "MagicRes");

        AddItemBouns(fireDamage_Modifer, "FIRE DAM");
        AddItemBouns(iceDamage_Modifer, "ICE DAM");
        AddItemBouns(lightingDamage_Modifer, "LIGHT DAM");

    
        stringBuilder.AppendLine();
        stringBuilder.Append("");

        return stringBuilder.ToString();
    }

    private void AddItemBouns(int modifer, string name)
    {   
        if(modifer != 0)
        {
            if(stringBuilder.Length > 0)
                stringBuilder.AppendLine();
            
            stringBuilder.Append(name + ": " + modifer.ToString());
        }
    }

}

