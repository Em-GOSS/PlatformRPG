using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "IceAndFire", menuName = "Data/Item/Item Effects/IceAndFire_Effect")]
public class IceAndFire_Effect : ItemEffect
{   
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private Vector2 moveVelocity;

    public override void ExecuteEffect(Transform _respawnPosition)
    {   
        Player player = PlayerManager.Instance.player;

        if(player.primaryAttackState.GetComboCounter() == 3)
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _respawnPosition.position,  player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = moveVelocity * player.FacingDirection;
            Destroy(newIceAndFire, 10f);
        }       
    }
}
