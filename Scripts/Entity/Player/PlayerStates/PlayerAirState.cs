using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0,0);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.SetVelocity(xInput*Settings.playerMoveSpeed,player.Rigidbody.velocity.y);
    }

    public override void Update()
    {
        base.Update();
        if(player.IsGroundDectected())
        {
            playerStateMachine.ChangeState(player.idleState);
        }
        
        if(player.IsWallDectected())
        {
            playerStateMachine.ChangeState(player.wallSlideState);
        }

    }
}
