using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine newContext, PlayerStateFactory newFactory) : base(newContext, newFactory) { }
    float _currSpeed;
    static readonly float _startMovingSpeedReduction = 2;
    float _angularDragSpeedPenalty = 0.0005f;
    Vector2 _prevDirection;

    public override void EnterState()
    {
        isRootState = true;
        _currSpeed = context.PlayerControl.PlayerStats.Speed - (context.PlayerControl.PlayerStats.Acceleration != 0 ? _startMovingSpeedReduction : 0);
        _prevDirection = context.Movement;
        _angularDragSpeedPenalty = _startMovingSpeedReduction / 360;
    }

    public override void UpdateState() 
    {
        //if (context.PlayerControl.PlayerStats.Acceleration != 0)
            //HandleAngularDrag();
        _prevDirection = context.Movement;
        Vector2 movement = context.Movement * _currSpeed;
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

        if (_currSpeed < context.PlayerControl.PlayerStats.Speed)
            _currSpeed += context.PlayerControl.PlayerStats.Acceleration * Time.deltaTime;
        else if (_currSpeed > context.PlayerControl.PlayerStats.Speed)
            _currSpeed = context.PlayerControl.PlayerStats.Speed;
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
    void HandleAngularDrag()
    {
        if (_currSpeed <= context.PlayerControl.PlayerStats.Speed - _startMovingSpeedReduction)
            return;
        var prevAngle = Utility.GetAngleFromPointInCircle(_prevDirection);
        var currAngle = Utility.GetAngleFromPointInCircle(context.Movement);
        var angleVariation = prevAngle - currAngle;
        if (angleVariation < 0)
            angleVariation *= -1;

        _currSpeed -= _angularDragSpeedPenalty * angleVariation;
    }
}
