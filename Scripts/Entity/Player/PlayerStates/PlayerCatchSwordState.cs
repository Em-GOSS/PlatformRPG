using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{   
    private Transform swordTransform;
    public PlayerCatchSwordState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
        
    }


    public override void Enter()
    {   
        swordTransform = player.sword.transform;
        PlayerManager.Instance.FlipPlayerWithPos(swordTransform.position);
        player.entityFX.DoScreenShake();         
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {   
        if(triggerCalled)
            playerStateMachine.ChangeState(player.idleState);
        base.Update();
    }
}
