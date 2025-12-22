using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine newContext, PlayerStateFactory newFactory) : base(newContext, newFactory) { }

    public override void EnterState() 
    {
        isRootState = true;
    }

    public override void UpdateState() 
    {
        CheckSwitchStates();
        context.PlayerControl.PlayerAnimator.ChangeAnim("Idle");
    }

    public override void OnCollisionEnter(Collision collision) { }

    public override void ExitState() { }

    public override void CheckSwitchStates() 
    {
        if(context.Movement != Vector2.zero)
        {
            SwitchState(factory.Run());

        }
    }

    public override void InitializeSubState() { }
}
