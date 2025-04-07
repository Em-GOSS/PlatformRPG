using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{   
    private Enemy_Skeleton skeleton;
    private Transform playerTransform;
    protected int moveDirection = 1;

    public SkeletonBattleState(Enemy enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName,Enemy_Skeleton _enemy) : base(enemyBase, enemyStateMachine, animBoolName)
    {
        skeleton=_enemy;
    }

    public override void Enter()
    {
        base.Enter();

        playerTransform = PlayerManager.Instance.player.transform;
        skeleton.StartCoroutine(skeleton.TransformNoticeWindow());
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();

        if(skeleton.IsPlayerDectected()) 
        {   
            stateTimer=skeleton.battleTime;

            if(skeleton.IsPlayerDectected().distance<=skeleton.attack_Distance)
            {   
                if(skeleton.canEnemyAttack())
                {   
                    skeleton.enemyAttackLastTime=Time.time;
                    enemyStateMachine.ChangeState(skeleton.skeletonAttackState);                    
                }
                    
            }
        }
        else
        {
            if(stateTimer<0 || Vector2.Distance(playerTransform.transform.position,skeleton.transform.position)>skeleton.maxChaseDistance) 
                enemyStateMachine.ChangeState(skeleton.skeletonIdleState);
        }

        Battle();
    }

    private void Battle() 
    {
        if(playerTransform.position.x < skeleton.transform.position.x)
            moveDirection = -1;
        else
            moveDirection = 1;
          
        if(skeleton.IsTimeFreeze())
            return;
        skeleton.SetVelocity(moveDirection*skeleton.battleMoveSpeed, skeleton.Rigidbody.velocity.y);
    }
}
