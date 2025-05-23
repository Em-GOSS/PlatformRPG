using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stateTimer=0.4f;
        player.SetVelocity(-Settings.playerWallJumpVerticalSpeed*player.FacingDirection,Settings.playerJumpSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer<0) 
        {
            playerStateMachine.ChangeState(player.airState);
        }
        if(player.IsGroundDectected())
        {
            playerStateMachine.ChangeState(player.idleState);
        }
    }

}
