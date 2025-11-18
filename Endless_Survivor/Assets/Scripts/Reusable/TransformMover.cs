using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMover
{
    public string id;
    public Vector2 direction;
    public float distance;
    public float speed;
    public Transform movedTr;
    public Transform destinationTarget;
    public Action onDistanceReached;
    float _lapsedDistance = 0;
    Vector2 _previousPos;

    public TransformMover(string id, Vector2 direction, float distance, float speed, Transform movedTr, Transform destinationTarget, Action onDistanceReached)
    {
        this.id = id;
        this.direction = direction;
        this.distance = distance;
        this.speed = speed;
        this.movedTr = movedTr;
        this.destinationTarget = destinationTarget;
        this.onDistanceReached = onDistanceReached;
        _previousPos = movedTr.position;
    }

    public void Update()
    {
        Vector2 deltaMovement = direction * speed * Time.deltaTime;
        movedTr.Translate(deltaMovement);

        _lapsedDistance += deltaMovement.magnitude;
        if(_lapsedDistance >= distance)
            onDistanceReached?.Invoke();
    }
}
