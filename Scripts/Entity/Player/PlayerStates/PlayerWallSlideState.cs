using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{   
    private float slowMultiple=0.3f;

    public PlayerWallSlideState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
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

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        player.SetVelocity(0,player.Rigidbody.velocity.y*slowMultiple);
    }

    public override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerStateMachine.ChangeState(player.wallJumpState);
        }
         
        if(xInput!=0&&player.FacingDirection!=xInput)
        {
            playerStateMachine.ChangeState(player.idleState);
        }
    
        if(player.IsGroundDectected())
        {
            playerStateMachine.ChangeState(player.idleState);
        }

        if(player.IsWallDectected()==false)
        {
            playerStateMachine.ChangeState(player.airState);
        }


        //Slide Speed Revision
        if(yInput<0) 
        {
            slowMultiple=Settings.playerQuickWallSlideMultiple;
        }
        else
        {
            slowMultiple=Settings.playerNormalWallSlideMultiple;
        }
    }
}
