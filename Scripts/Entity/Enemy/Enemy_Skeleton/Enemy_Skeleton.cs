using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{   
    #region states 

    public SkeletonIdleState skeletonIdleState;
    public SkeletonMoveState skeletonMoveState;
    public SkeletonBattleState skeletonBattleState;
    public SkeletonAttackState skeletonAttackState;
    public SkeletonStunnedState skeletonStunnedState;
    public SkeletonDeadState skeletonDeadState;

    #endregion

    public override void Awake()
    {
        base.Awake();

        skeletonIdleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        skeletonMoveState = new SkeletonMoveState(this, stateMachine, "Move", this);
        skeletonBattleState = new SkeletonBattleState(this, stateMachine, "Battle", this);
        skeletonAttackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
        skeletonStunnedState = new SkeletonStunnedState(this, stateMachine, "Stunned", this);
        skeletonDeadState = new SkeletonDeadState(this, stateMachine, "Idle", this);
    }

    public override void Start()
    {
        base.Start();

        stateMachine.Initialize(skeletonIdleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    private void SkeletonStunned()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            stateMachine.ChangeState(skeletonStunnedState);
        }
    }

    public override bool CanBeStunned()
    {
        if(base.CanBeStunned()) 
        {
            stateMachine.ChangeState(skeletonStunnedState);
            return true;
        }

        return false;
    }

    public override void Die()
    {   
        if(isDead)
            return;
        base.Die();
        stateMachine.ChangeState(skeletonDeadState);
    }

}
