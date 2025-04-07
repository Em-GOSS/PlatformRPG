using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{       

    private int comboCounter = 1;

    private float lastTimeAttacked;
    private float comboWindow=Settings.PlayerPrimaryAttackWindow;

    private int attackAmount=Settings.PlayerPrimaryAttackAmount;
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();

        if(Time.time-lastTimeAttacked>comboWindow)
            comboCounter=1;
        if(comboCounter>attackAmount)
            comboCounter=1;

        player.animator.SetInteger("ComboCounter",comboCounter);

        player.animator.speed=1f;
        
        float attackDirection=player.FacingDirection; 

        xInput=Input.GetAxisRaw("Horizontal");
        if(xInput!=0)
            attackDirection=xInput;

        player.SetVelocity(
            Settings.PlayerAttackMovementxSpeed[comboCounter]*attackDirection,
            Settings.PlayerAttackMovementySpeed[comboCounter]
        );
    }
    

    public override void Exit()
    {
        base.Exit();
        player.animator.speed=1f;
        comboCounter++;
        lastTimeAttacked=Time.time;
        player.StartCoroutine(player.BusyFor(0.15f));
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        
        if(triggerCalled)
            playerStateMachine.ChangeState(player.idleState);
    }

    public int GetComboCounter()
    {
        return comboCounter;
    }
}
