using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(xInput==0) 
        {   
            player.SetVelocity(0,player.Rigidbody.velocity.y);
            playerStateMachine.ChangeState(player.idleState);
        }
        if(xInput==player.FacingDirection&&player.IsWallDectected())
        {
            playerStateMachine.ChangeState(player.idleState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if(xInput!=0)
        {
            player.SetVelocity(xInput*player.moveSpeed,player.Rigidbody.velocity.y);
        }
        
        
    }

}
