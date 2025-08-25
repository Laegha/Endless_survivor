using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine newContext, PlayerStateFactory newFactory) : base(newContext, newFactory) { }

    public override void EnterState()
    {
        isRootState = true;
    }

    public override void UpdateState() 
    {
        Vector2 movement = context.Movement * context.PlayerControl.PlayerStats.Speed;
        var animMovement = movement.normalized;
        if(Mathf.Abs(animMovement.y) >= Mathf.Abs(animMovement.x))
        {
            if(animMovement.y < 0)
                context.PlayerControl.PlayerAnimator.ChangeAnim("FrontMove");
            else
                context.PlayerControl.PlayerAnimator.ChangeAnim("BackMove");
        }
        else
        {
            if(animMovement.x > 0)
                context.PlayerControl.PlayerAnimator.ChangeAnim("RightMove");
            else
            context.PlayerControl.PlayerAnimator.ChangeAnim("LeftMove");
        }
        context.PlayerControl.PlayerRb.velocity = movement;
        CheckSwitchStates();
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
