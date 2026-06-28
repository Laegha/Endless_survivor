using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

[Serializable]
public class ThrowAtEnemySupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] CustomAnimation _thrownAnimation;
    [SerializeField] float _throwSpeed = 10;
    [SerializeField] float _collisionCheckRadius = .2f;
    [SerializeField] LayerMask _collidedLayers;
    [SerializeField] SFXInfo _onCollisionSFX; 
    //[SerializeField CustomAnimation _windupAnimation;
    [SerializeField] AnimationCurve _verticalMovementCurve;
    Vector2 _initialPos;
    Vector2 _throwDirection;
    Vector2 _verticalDirection;
    float _totalDistance;
    float _lapsedDistance;


    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var throwOriginal = original as ThrowAtEnemySupportObjBehaviour;
        _thrownAnimation = throwOriginal._thrownAnimation;
        _throwSpeed = throwOriginal._throwSpeed;
        _collisionCheckRadius = throwOriginal._collisionCheckRadius;
        _collidedLayers = throwOriginal._collidedLayers;
        _onCollisionSFX = throwOriginal._onCollisionSFX;
        _verticalMovementCurve = throwOriginal._verticalMovementCurve;

        ObjControl.Animator.AddAnimations(new(){_thrownAnimation});
        _initialPos = control.transform.position;
        OnStart += CalculateDirection;
        OnStart += () => ObjControl.Animator.ChangeAnim(_thrownAnimation.AnimationName);
        OnUpdate += MoveTowardsEnemy;
    }
    void CalculateDirection()
    {
        var closestEnemy = Utility.GetClosestTo(EnemySpawnManager.esm.Enemies, PlayerControl.pc.transform)[0];
        var throwVector = closestEnemy.transform.position - ObjControl.transform.position;
        _throwDirection = throwVector.normalized;
        _totalDistance = throwVector.magnitude;

        _verticalDirection = Utility.GetPerpendicularVector(_throwDirection);
    }
    void MoveTowardsEnemy()
    {
        Vector2 xMovement = _throwDirection * _lapsedDistance;
        Vector2 yMovement = _verticalDirection * _verticalMovementCurve.Evaluate(Mathf.Clamp01(_lapsedDistance / _totalDistance));
        ObjControl.transform.position = _initialPos + xMovement + yMovement;

        float distanceDelta = Time.deltaTime * _throwSpeed;
        _lapsedDistance += distanceDelta;

        var collidingObjs = Physics2D.OverlapCircleAll(ObjControl.transform.position, _collisionCheckRadius, _collidedLayers);
        if(collidingObjs.Length > 0)
        {
            SoundFXManager.sm.PlaySfx(_onCollisionSFX, ObjControl.transform.position);
            GameObject.Destroy(ObjControl.gameObject);
            return;
        }
        if(_lapsedDistance < _totalDistance)
            return;
        ObjControl.transform.position = _initialPos + _throwDirection * _totalDistance;
        GameObject.Destroy(ObjControl.gameObject);
    }
}
