using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{   
    private Enemy enemy;
    private ItemDrop dropSystem;

    [Header("Level Details")]
    [SerializeField] private int level = 1;

    [Range(0f,1f)]
    [SerializeField] private float percentageModifer = 0.1f;

    protected override void Start()
    {   
        ApplyLevelModifers();
        base.Start();
        enemy = this.GetComponent<Enemy>();
        dropSystem = this.GetComponent<ItemDrop>();
    }

    private void ApplyLevelModifers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);
    }

    private void Modify(Stat _stat)
    {   
        for(int i = 0; i < level; i++)
        {
            int modifer = Mathf.RoundToInt(_stat.GetValue() * percentageModifer);
            _stat.AddModifer(modifer);
        }   
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {   
        if(enemy.isDead)
            return;
        base.Die();
        dropSystem.GenerateDrop();
        enemy.Die();
        
       
    }
}
