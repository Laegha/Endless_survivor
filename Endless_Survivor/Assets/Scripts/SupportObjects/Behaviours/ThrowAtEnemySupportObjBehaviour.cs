using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ThrowAtEnemySupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] CustomAnimation _thrownAnimation;
    [SerializeField] float _throwSpeed = 10;
    [SerializeField] float _collisionCheckRadius = .2f;
    [SerializeField] LayerMask _collidedLayers;
    //[SerializeField CustomAnimation _windupAnimation;

    Vector2 _throwDirection;
    float _totalDistance;
    float _lapsedDistance;

    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var throwOriginal = original as ThrowAtEnemySupportObjBehaviour;
        _throwSpeed = throwOriginal._throwSpeed;
        _collisionCheckRadius = throwOriginal._collisionCheckRadius;
        _collidedLayers = throwOriginal._collidedLayers;
        OnStart += CalculateDirection;
        OnUpdate += MoveTowardsEnemy;
    }
    void CalculateDirection()
    {
        var closestEnemy = Utility.GetClosestTo(WaveManager.wm.Enemies, PlayerControl.pc.transform)[0];
        var throwVector = closestEnemy.transform.position - ObjControl.transform.position;
        _throwDirection = throwVector.normalized;
        _totalDistance = throwVector.magnitude;
    }
    void MoveTowardsEnemy()
    {
        Vector2 movementDelta = _throwDirection * Time.deltaTime * _throwSpeed;
        ObjControl.transform.Translate(movementDelta);
        _lapsedDistance += movementDelta.magnitude;
        var collidingObjs = Physics2D.OverlapCircleAll(ObjControl.transform.position, _collisionCheckRadius, _collidedLayers);
        if(collidingObjs.Length > 0)
        {
            GameObject.Destroy(ObjControl.gameObject);
            return;
        }
        if(_lapsedDistance >= _totalDistance)
            return;
        GameObject.Destroy(ObjControl.gameObject);
    }
}
