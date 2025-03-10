using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine newContext, PlayerStateFactory newFactory) : base(newContext, newFactory) { }

    public override void EnterState()
    {
        isRootState = true;
        context.PlayerControl.PlayerAnimator.ChangeAnim("Moving");
    }

    public override void UpdateState() 
    {
        CheckSwitchStates();

        Vector2 movement = context.Movement * context.PlayerControl.PlayerStats.Speed;
        context.PlayerControl.PlayerAnimator.SetMovement(movement);
        context.PlayerControl.PlayerRb.velocity = movement;
    }

    public override void OnCollisionEnter(Collision collision) { }

    public override void ExitState() { }

    public override void CheckSwitchStates() 
    {
        if (context.Movement == Vector2.zero)
            SwitchState(factory.Idle());
    }

    public override void InitializeSubState() { }
}
