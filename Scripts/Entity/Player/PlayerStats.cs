using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{   
    Player player;

    protected override void Start()
    {
        base.Start();
        player = this.GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

    }

    protected override void Die()
    {
        base.Die();
        player.Die();
    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);

        ItemData_Equipment currentArmor = Inventory.Instance.GetEquipment(EquipmentType.Armor);
        if(currentArmor != null)
        {
            currentArmor.ProcessItemEffects(player.transform);
        }
    }
}
