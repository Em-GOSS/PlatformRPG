using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{   
    private bool canCreateClone = false;

    public PlayerCounterAttackState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        canCreateClone = true;
        stateTimer = player.counterAttackDuartion;
        player.animator.SetBool("SuccessfulCounterAttack",false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        Collider2D[] hits = Physics2D.OverlapCircleAll(player.attack_Check.position,player.attack_CheckRadius);

        foreach(Collider2D hit in hits)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                if(hit.GetComponent<Enemy>().CanBeStunned())
                {   
                    if(hit.GetComponent<Enemy>().isAttacking==true)

                    stateTimer = 10;
                    
                    player.animator.SetBool("SuccessfulCounterAttack",true);

                    if(canCreateClone)
                    {
                        canCreateClone = false;
                        player.skillManager.clone_Skill.CreateCloneOnCounterAttack(hit.transform);
                    }
                    
                }
            }
        }
        

        if(stateTimer<0 || triggerCalled )
        {
            playerStateMachine.ChangeState(player.idleState);
        }
    }


}
