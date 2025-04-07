using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        stateTimer=Settings.playerDashDuartion;
        player.skillManager.clone_Skill.CreateCloneOnDashStart();
    }

    public override void Exit()
    {
        base.Exit();
        player.skillManager.clone_Skill.CreateCloneOnDashOver();
        player.SetZeroVelocity();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.SetVelocity(player.dashSpeed * player.playerDashDirection,0);
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer<0)
        {
            playerStateMachine.ChangeState(player.idleState);
        }
        if(player.IsWallDectected())
        {
            playerStateMachine.ChangeState(player.wallSlideState);
        }
    }
}
