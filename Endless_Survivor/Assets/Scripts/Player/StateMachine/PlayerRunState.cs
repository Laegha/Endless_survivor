using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine newContext, PlayerStateFactory newFactory) : base(newContext, newFactory) { }

    public override void EnterState()
    {
        context.PlayerAnimator.Play("Run");
    }

    public override void UpdateState() 
    {
        CheckSwitchStates();

        Vector2 movement = (context.Movement * context.RunSpeed * Time.deltaTime).normalized;
        context.PlayerAnimator.SetFloat("Horizontal", movement.x);
        context.PlayerAnimator.SetFloat("Vertical", movement.y);
        context.PlayerRb.velocity = new Vector3(movement.x, movement.y);
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
