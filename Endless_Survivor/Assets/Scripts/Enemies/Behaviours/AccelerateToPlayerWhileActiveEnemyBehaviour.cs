using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AccelerateToPlayerWhileActiveEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] float _acceleration;
    [SerializeField] float _initialSpeed;
    [SerializeField] float _maxSpeed;
    [SerializeField] bool _instantStopOnUnactive;
    [Tooltip("Only use if instantStopOnUnactive")][SerializeField] float _deacceleration;
    [SerializeField] DirectionalCustomAnimation _movingAnimations;

    bool _isDeaccelerating;
    float _speed;
    Vector2 _lastDirection;

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var accelerateOriginal = original as AccelerateToPlayerWhileActiveEnemyBehaviour;
        _acceleration = accelerateOriginal._acceleration;
        _initialSpeed = accelerateOriginal._initialSpeed;
        _maxSpeed = accelerateOriginal._maxSpeed;
        _instantStopOnUnactive = accelerateOriginal._instantStopOnUnactive;
        _deacceleration = accelerateOriginal._deacceleration;

        _movingAnimations = new(EnemyControl.Animator, accelerateOriginal._movingAnimations);
        EnemyControl.Animator.AddAnimations(_movingAnimations.NonNullAnimations);
    }

    public override void PassiveUpdate()
    {
        base.PassiveUpdate();
        if (_instantStopOnUnactive)
            return;
        if(IsActive || !_isDeaccelerating)
            return;
        _speed -= _deacceleration * Time.deltaTime;
        EnemyControl.RbForcesController.ChangeCurrForce(new(_lastDirection, _speed, 0, ForceMode2D.Force));
        if(_speed <= 0)
        {
            _isDeaccelerating = false;
            _speed = _initialSpeed;
            EnemyControl.Animator.EndAnimation(_movingAnimations.GetAnim(_lastDirection));
        }

    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        Vector2 direction = (PlayerControl.pc.transform.position - EnemyControl.transform.position).normalized;
        _lastDirection = direction;

        var currAnim = _movingAnimations.GetAnim(direction);
        if (EnemyControl.Animator.CurrAnim != null && currAnim.AnimationName != EnemyControl.Animator.CurrAnim.AnimationName && _movingAnimations.NonNullAnimations.Any(x => x.AnimationName == EnemyControl.Animator.CurrAnim.AnimationName))
            EnemyControl.Animator.EndAnimation(EnemyControl.Animator.CurrAnim.AnimationName);

        EnemyControl.Animator.ChangeAnim(currAnim.AnimationName);

        EnemyControl.RbForcesController.ChangeCurrForce(new(direction, _speed, 0, ForceMode2D.Force));
        _speed += _acceleration * Time.deltaTime;
        _speed = Mathf.Clamp(_speed, _initialSpeed, _maxSpeed);
    }
    public override void KillBehaviour()
    {
        base.KillBehaviour();
        if (_instantStopOnUnactive)
        {
            _speed = _initialSpeed;
            GameManager.gm.DelayActionAFrame(EnemyControl.RbForcesController.Stop, () => EnemyControl == null);//this one is to actually stop the enemy, delayed a frame to sort the frame delay with the last active update
            if (_movingAnimations.NonNullAnimations.Any(anim => anim.AnimationName == EnemyControl.Animator.CurrAnim.AnimationName))
                EnemyControl.Animator.EndAnimation(EnemyControl.Animator.CurrAnim.AnimationName);
        }
        else
        {
            EnemyControl.RbForcesController.Stop();//this one is to override the movement force
            _isDeaccelerating = true;

        }
    }
}
