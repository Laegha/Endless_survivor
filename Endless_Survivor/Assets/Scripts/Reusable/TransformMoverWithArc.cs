using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMoverWithArc
{
    Vector2 _initialPos;
    Vector2 _endPos;
    AnimationCurve _verticalMovementCurve;
    Transform _movingObj;
    float _moveSpeed;

    Vector2 _horizontalDirection;
    Vector2 _verticalDirection;

    float _totalDistance;
    float _lapsedDistance = 0;

    bool _movementEnded;

    public bool MovementEnded { get { return _movementEnded; } }

    public TransformMoverWithArc(Vector2 initialPos, Vector2 endPos, AnimationCurve verticalMovementCurve, Transform movingObj, float moveSpeed)
    {
        _initialPos = initialPos;
        _endPos = endPos;
        _verticalMovementCurve = verticalMovementCurve;
        _movingObj = movingObj;
        _moveSpeed = moveSpeed;

        Vector2 movement = _endPos - _initialPos;
        _horizontalDirection = movement.normalized;
        _verticalDirection = Utility.GetPerpendicularVector(_horizontalDirection);
        _totalDistance = movement.magnitude;
    }

    public void Move()
    {
        if(_movementEnded) 
            return;
        Vector2 xMovement = _horizontalDirection * _lapsedDistance;
        Vector2 yMovement = _verticalDirection * _verticalMovementCurve.Evaluate(Mathf.Clamp01(_lapsedDistance / _totalDistance));
        _movingObj.position = _initialPos + xMovement + yMovement;

        float distanceDelta = Time.deltaTime * _moveSpeed;
        _lapsedDistance += distanceDelta;

        if (_lapsedDistance < _totalDistance)
            return;
        _movingObj.position = _initialPos + _horizontalDirection * _totalDistance;
        _movementEnded = true;
    }
}
