using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
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
    }

    public override void Update()
    {
        base.Update();

        if(!player.IsGroundDectected())
        {
            playerStateMachine.ChangeState(player.airState);
        }

        if(Input.GetKeyDown(KeyCode.Space)&&player.IsGroundDectected())
        {
            playerStateMachine.ChangeState(player.jumpState);
        }

        if(Input.GetKey(KeyCode.Mouse0))
        {
            playerStateMachine.ChangeState(player.primaryAttackState);
        }

        //Sword Skill
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {   
            if(!player.SwordCanReturn())
            {
                playerStateMachine.ChangeState(player.aimSwordState);
            }         
        }

        //BlackHole Skill 
        if(Input.GetKeyDown(KeyCode.R))
        {   
            if(player.skillManager.blackhole_Skill.IsSkillCharingOver())
                playerStateMachine.ChangeState(player.blackholeState);
        }
        //Counter Attack
        if(Input.GetKeyDown(KeyCode.Q))
        {
            playerStateMachine.ChangeState(player.counterAttackState);
        }
    }
}
