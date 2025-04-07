using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundedState
{   
    public SkeletonMoveState(Enemy enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, Enemy_Skeleton _enemy) : base(enemyBase, enemyStateMachine, animBoolName, _enemy)
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
        
        skeleton.SetVelocity(skeleton.moveSpeed*skeleton.FacingDirection,skeleton.Rigidbody.velocity.y);

        if(skeleton.IsWallDectected() || !skeleton.IsGroundDectected()) 
        {   
            skeleton.StartCoroutine(WaitingFlip());    
            skeleton.stateMachine.ChangeState(skeleton.skeletonIdleState);
        }
        

    }

    private IEnumerator WaitingFlip()
    {
        yield return new WaitForSeconds(skeleton.idleTime);
        skeleton.FlipDirection();
    }
}
