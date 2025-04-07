using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{       
    public bool isBusy{get; private set;}

    public PlayerStateMachine stateMachine{get; private set;}
    public PlayerIdleState idleState{get; private set;} 
    public PlayerMoveState moveState{get; private set;}
    public PlayerJumpState jumpState{get; private set;}
    public PlayerAirState airState{get; private set;}
    public PlayerDashState dashState{get; private set;}
    public PlayerWallSlideState wallSlideState{get; private set;}
    public PlayerWallJumpState wallJumpState{get; private set;}

    public PlayerPrimaryAttackState primaryAttackState{get; private set;}
    public PlayerCounterAttackState counterAttackState{get; private set;}

    //Skills' States
    public PlayerAimSwordState aimSwordState{get; private set;}
    public PlayerCatchSwordState catchSwordState{get; private set;}
    public PlayerBlackholeState blackholeState{get; private set;}

    //Dead State
    public PlayerDeadState deadState{get; private set;}

    //Player situation
    public float playerDashDirection{get; private set;}

    //Convient_SkillManager
    public SkillManager skillManager;
        //Sowrd skill
        public GameObject sword{get; private set;}


    [Header("Counter Attack Info")]
    public float counterAttackDuartion;


    public float moveSpeed {get; private set;}
    public float jumpForce {get; private set;}
    public float dashSpeed {get; private set;}
    

    public override void Awake() 
    {       
        stateMachine=new PlayerStateMachine();

        idleState = new PlayerIdleState(this, this.stateMachine, "Idle");
        moveState = new PlayerMoveState(this, this.stateMachine, "Move");
        jumpState = new PlayerJumpState(this, this.stateMachine, "Jump");
        airState = new PlayerAirState(this, this.stateMachine, "Jump");
        dashState = new PlayerDashState(this, this.stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, this.stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, this.stateMachine, "Jump");

        primaryAttackState = new PlayerPrimaryAttackState(this, this.stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, this.stateMachine, "CounterAttack");

        aimSwordState = new PlayerAimSwordState(this, this.stateMachine, "AimSword");
        catchSwordState = new PlayerCatchSwordState(this, this.stateMachine, "CatchSword");
        blackholeState = new PlayerBlackholeState(this, this.stateMachine, "Jump");
        
        deadState = new PlayerDeadState(this,this.stateMachine,"Die");
        
        //data LoadIn
        moveSpeed = Settings.playerMoveSpeed;
        jumpForce = Settings.playerJumpSpeed;
        dashSpeed = Settings.playerDsahSpeed;

    }

    public override void Start() 
    {   
        base.Start();
        skillManager = SkillManager.Instance;
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {   
        if(isDead)
            return;
        stateMachine.currentState.Update();
        CheckForDashInput();
        CheckForSwordInput();
        CheckForCrystalInput();

        if(Input.GetKeyDown(KeyCode.Alpha1))
            Debug.Log("Use Flask Heal");
    }

    private void FixedUpdate()
    {
        stateMachine.currentState.FixedUpdate();
    }   

    private void CheckForDashInput() 
    {   
        if(IsWallDectected())
            return;

        if(Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.Instance.dash_Skill.TryToUseSkill())
        {       
            playerDashDirection=Input.GetAxisRaw("Horizontal");
            
            if(playerDashDirection==0)
                playerDashDirection=FacingDirection;

            stateMachine.ChangeState(dashState);
        }
    }  
 
    private void CheckForSwordInput() 
    {   
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {   
            if(!SwordCanReturn())
                return;

            Sword_Skill_Controller sword_Skill_Controller = sword.GetComponent<Sword_Skill_Controller>();
            if(sword_Skill_Controller!=null)
            {
                if(sword_Skill_Controller.canStopSpinning)
                {
                    sword_Skill_Controller.Spinning();
                }
                else
                {
                    sword_Skill_Controller.ReturnSword();
                }     
            }
            
        }       
    }


    private void CheckForCrystalInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            skillManager.crystal_Skill.TryToUseSkill();
        }
    }



    //Sword Skills
    public void AssignNewSword(GameObject newSword) 
    {
        sword = newSword;
    }

    public void CatchSword() 
    {   
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }

    public bool SwordCanReturn()
    {
        if(sword == null)
            return false;
        return true;
    }

    public override void Die()
    {   
        if(isDead)
            return;
        base.Die();
        stateMachine.ChangeState(deadState);
        GetComponent<Player_ItemDrop>()?.GenerateDrop();  
    }

    //Animation situation 
    public void AnimationTrigger()=>stateMachine.currentState.AnimationFinishTrigger();

    public override void SlowEntityBy(float _slowPercentage, float slowDuartion)
    {
        moveSpeed *= (1 - _slowPercentage);
        dashSpeed *= (1 - _slowPercentage);
        jumpForce *= (1 - _slowPercentage);


        Invoke("ReturnDefaultSpeed", slowDuartion);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = Settings.playerMoveSpeed;
        jumpForce = Settings.playerJumpSpeed;
        dashSpeed = Settings.playerDsahSpeed;
    }

    //Busy Settings 
    public IEnumerator BusyFor(float seconds) 
    {
        isBusy=true;

        yield return new WaitForSeconds(seconds);

        isBusy=false;
    }
}
