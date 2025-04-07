using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{   
    private float flyTime = .4f;
    private bool skillUsed;
    private float defaultGravity;


    public PlayerBlackholeState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
        
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        defaultGravity = player.Rigidbody.gravityScale;

        skillUsed = false;
        stateTimer = flyTime;
        player.Rigidbody.gravityScale = 0;
        
    }

    public override void Exit()
    {
        base.Exit();
        player.Rigidbody.gravityScale = defaultGravity;
        player.entityFX.MakeTransparent(false);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer > 0)
            player.Rigidbody.velocity = new Vector2(0, 8);

        if(stateTimer < 0)
        {
            player.Rigidbody.velocity = new Vector2(0, -0.1f);

            if(!skillUsed)
            {
                if(player.skillManager.blackhole_Skill.TryToUseSkill())
                    skillUsed = true;
            }
        }

        if(player.skillManager.blackhole_Skill.SkillCompleted())
        {
            player.stateMachine.ChangeState(player.airState);
        }

    }
}
