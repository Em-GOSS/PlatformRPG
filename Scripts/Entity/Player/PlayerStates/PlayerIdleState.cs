public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(xInput==player.FacingDirection&&player.IsWallDectected())
            return;

        if(xInput!=0&&player.isBusy==false)
        {
            playerStateMachine.ChangeState(player.moveState);
        }
    }
}
