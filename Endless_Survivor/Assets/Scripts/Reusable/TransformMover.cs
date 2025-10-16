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
    public Action onDistanceReached;
    float _lapsedDistance = 0;

    public TransformMover(string id, Vector2 direction, float distance, float speed, Transform movedTr, Action onDistanceReached)
    {
        this.id = id;
        this.direction = direction;
        this.distance = distance;
        this.speed = speed;
        this.movedTr = movedTr;
        this.onDistanceReached = onDistanceReached;
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
