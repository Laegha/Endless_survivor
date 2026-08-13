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
        context.PlayerControl.PlayerAnimator.ChangeAnimButKeepFrame("Idle");
        var dirIndicatorAN = context.PlayerControl.DirIndicatorAN;
        dirIndicatorAN.ChangeAnim(context.PlayerControl.DirIndicatorIdleName);
        dirIndicatorAN.transform.rotation = Quaternion.identity;
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
