using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
        List<Tile> possibleDestinationTiles = MapManager.mm.LoadedTiles.Where((tile) =>
        {
            float tileDist = Vector2.Distance(tile.transform.position, ObjControl.transform.position);
            return tileDist > _movementDistance.min && tileDist < _movementDistance.max && !tile.IsWall;
        }
        ).ToList();
        Vector2 destination = possibleDestinationTiles[Random.Range(0, possibleDestinationTiles.Count)].transform.position;
        Vector2 movement = destination - (Vector2)ObjControl.transform.position;
        _currMovingDirection = movement.normalized;
        _currMovingDistance = Mathf.Clamp(_movementDistance.rand, 0, movement.magnitude);
        _currMovingSpeed = _movementSpeed.rand;
        _currLapsedDistance = 0;
        _isMoving = true;
        foreach (var renderer in ObjControl.Renderers)
        {
            renderer.flipX = _currMovingDirection.x < 0;
        }
        //Apply speed to rb instead of translating transform in move?
    }
    void Move()
    {
        if (!_isMoving)
        {
            ObjControl.Animator.ChangeAnim(ObjControl.BehaviourManager.SupportObjData.IdleAnimation.AnimationName);
            return;
        }
        if (ObjControl.Animator.CurrAnim.AnimationName != _movingAnimation.AnimationName)
        {
            ObjControl.Animator.ChangeAnim(_movingAnimation);
            return;
        }
        var movementDelta = Time.deltaTime * _currMovingSpeed;
        _currLapsedDistance += movementDelta;
        var movement = _currMovingDirection * movementDelta;
        ObjControl.transform.Translate(movement);
        if(_currLapsedDistance > _currMovingDistance)
        {
            //end animation
            ObjControl.Animator.EndAnimation(_movingAnimation);
            _isMoving = false;
            _onMovementEnded?.Invoke();
        }
    }
}
