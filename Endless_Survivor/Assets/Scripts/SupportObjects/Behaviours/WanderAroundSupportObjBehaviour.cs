using UnityEngine;
using System.Collections.Generic;

public class WanderAroundSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] RandomBetweenTwoConstants _movementSpeed;
    [SerializeField] RandomBetweenTwoConstants _movementDistance;
    [SerializeField] RandomBetweenTwoConstants _timeBetweenMovements;
    [SerializeField] CustomAnimation _movingAnimation;
    float _timer;
    Vector2 _currMovingDirection;
    float _currMovingSpeed;
    float _currMovingDistance;
    bool _isMoving;
    float _currLapsedDistance;
    System.Action _onMovementEnded;

    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var wanderAroundOriginal = original as WanderAroundSupportObjBehaviour;
        _movementSpeed = wanderAroundOriginal._movementSpeed;
        _movementDistance = wanderAroundOriginal._movementDistance;
        _timeBetweenMovements = wanderAroundOriginal._timeBetweenMovements;
        _timer = _timeBetweenMovements.rand;
        _movingAnimation = wanderAroundOriginal._movingAnimation;
        ObjControl.Animator.AddAnimations(new List<CustomAnimation> { _movingAnimation });
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
        Vector2 randDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        Vector2 maxEndPoint = (Vector2)ObjControl.transform.position + randDirection * _movementDistance.max;
        while(maxEndPoint.x < MapMinBound.x || maxEndPoint.y < MapMinBound.y || maxEndPoint.x > MapMaxBound.x || maxEndPoint.y > MapMaxBound.y)
        {
            randDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            maxEndPoint = (Vector2)ObjControl.transform.position + randDirection * _movementDistance.max;
        }
        _currMovingDirection = randDirection;
        _currMovingDistance = _movementDistance.rand;
        _currMovingSpeed = _movementSpeed.rand;
        _currLapsedDistance = 0;
        _isMoving = true;
        ObjControl.Animator.ChangeAnim(_movingAnimation);
        foreach (var renderer in ObjControl.Renderers)
        {
            renderer.flipX = _currMovingDirection.x < 0;
        }
        //Apply speed to rb instead of translating transform in move?
    }
    void Move()
    {
        if (!_isMoving)
            return;
        var movementDelta = Time.deltaTime * _currMovingSpeed;
        _currLapsedDistance += movementDelta;
        var movement = _currMovingDirection * movementDelta;
        ObjControl.transform.Translate(movement);
        if(_currLapsedDistance > _currMovingDistance)
        {
            //end animation
            ObjControl.Animator.EndAnimation(_movingAnimation);
            ObjControl.Animator.ChangeAnim("Idle");
            _isMoving = false;
            _onMovementEnded?.Invoke();
        }
    }
}
