using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {   
        base.Enter();
        player.SetZeroVelocity();
        SkillManager.Instance.sword_Skill.SetDots(true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            player.stateMachine.ChangeState(player.idleState);
        }

        PlayerManager.Instance.FlipPlayerWithPos(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        
    }
}
