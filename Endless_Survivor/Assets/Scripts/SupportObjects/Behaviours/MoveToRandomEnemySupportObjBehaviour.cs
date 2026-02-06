using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveToRandomEnemySupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] RandomBetweenTwoConstants _movementSpeed;
    [SerializeField] float _stopDistance;
    [SerializeField] RandomBetweenTwoConstants _timeBetweenMovements;
    [SerializeField] CustomAnimation _movingUpAnimation;
    [SerializeField] CustomAnimation _movingRightAnimation;
    [SerializeField] CustomAnimation _movingDownAnimation;
    [SerializeField] CustomAnimation _movingLeftAnimation;

    float _timer;

    Vector2 _currMovingDirection;
    float _currMovingSpeed;
    float _currMovingDistance;
    bool _isMoving;
    float _currLapsedDistance;
    CustomAnimation _currAnim;

    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var moveToEnemyOriginal = original as MoveToRandomEnemySupportObjBehaviour;
        _movementSpeed = moveToEnemyOriginal._movementSpeed;
        _stopDistance = moveToEnemyOriginal._stopDistance;
        _timeBetweenMovements = moveToEnemyOriginal._timeBetweenMovements;

        _timer = _timeBetweenMovements.rand;
        _movingUpAnimation = moveToEnemyOriginal._movingUpAnimation;
        _movingRightAnimation = moveToEnemyOriginal._movingRightAnimation;
        _movingDownAnimation = moveToEnemyOriginal._movingDownAnimation;
        _movingLeftAnimation = moveToEnemyOriginal._movingLeftAnimation;

        List<CustomAnimation> addedAnimations = new();
        if (_movingUpAnimation.Frames.Length > 0)
            addedAnimations.Add(_movingUpAnimation);
        if (_movingRightAnimation.Frames.Length > 0)
            addedAnimations.Add(_movingRightAnimation);
        if (_movingDownAnimation.Frames.Length > 0)
            addedAnimations.Add(_movingDownAnimation);
        if (_movingLeftAnimation.Frames.Length > 0)
            addedAnimations.Add(_movingLeftAnimation);

        ObjControl.Animator.AddAnimations(addedAnimations);
        OnUpdate += DecreaseMovementTimer;
        OnUpdate += Move;
    }
    void DecreaseMovementTimer()
    {
        if (_isMoving)
            return;
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            StartMovingInRandomDirection();
            _timer = _timeBetweenMovements.rand;
        }
    }
    void StartMovingInRandomDirection()
    {
        GameObject randEnemy = EnemySpawnManager.esm.Enemies[Random.Range(0, EnemySpawnManager.esm.Enemies.Count)];
        if (randEnemy == null)
            return;
        Vector2 movement = randEnemy.transform.position - ObjControl.transform.position;

        _currMovingDirection = movement.normalized;
        _currMovingDistance = movement.magnitude - _stopDistance;
        _currMovingSpeed = _movementSpeed.rand;
        _currLapsedDistance = 0;
        _isMoving = true;

        _currAnim = GetAnimationFromDirection();
        //Apply speed to rb instead of translating transform in move?
    }
    void Move()
    {
        if (!_isMoving)
        {
            ObjControl.Animator.ChangeAnim(ObjControl.BehaviourManager.SupportObjData.IdleAnimation.AnimationName);
            return;
        }
        if (ObjControl.Animator.CurrAnim.AnimationName != _currAnim.AnimationName)
        {
            ObjControl.Animator.ChangeAnim(_currAnim);
            return;
        }
        var movementDelta = Time.deltaTime * _currMovingSpeed;
        _currLapsedDistance += movementDelta;
        var movement = _currMovingDirection * movementDelta;
        ObjControl.transform.Translate(movement);
        if (_currLapsedDistance > _currMovingDistance)
        {
            //end animation
            ObjControl.Animator.EndAnimation(_currAnim);
            _isMoving = false;
        }
    }
    //made with chatGPT
    CustomAnimation GetAnimationFromDirection()
    {
        float absX = Mathf.Abs(_currMovingDirection.x);
        float absY = Mathf.Abs(_currMovingDirection.y);

        // Primary axis: Horizontal
        if (absX > absY)
        {
            CustomAnimation horizontal =
                _currMovingDirection.x > 0 ? _movingRightAnimation : _movingLeftAnimation;

            if (horizontal.Frames.Length > 0)
                return horizontal;

            // Fallback to vertical
            return _currMovingDirection.y > 0 ? _movingUpAnimation : _movingDownAnimation;
        }
        // Primary axis: Vertical (includes absX == absY case)
        else
        {
            CustomAnimation vertical =
                _currMovingDirection.y > 0 ? _movingUpAnimation : _movingDownAnimation;

            if (vertical.Frames.Length > 0)
                return vertical;

            // Fallback to horizontal
            return _currMovingDirection.x > 0 ? _movingRightAnimation : _movingLeftAnimation;
        }
    }
}
