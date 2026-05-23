using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class FollowTrSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 0;
    [SerializeField] DirectionalCustomAnimation _movingAnimations;
    [SerializeField] float _stopDist;
    [SerializeField] Vector2 _targetPosOffset;
    Transform _target;
    Func<float> _moveSpeed;

    public Transform Target { set { _target = value; } }
    public Func<float> MoveSpeed {  set { _moveSpeed = value; } }

    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var followTrOriginal = original as FollowTrSupportObjBehaviour;
        _movingAnimations = new(control.Animator, followTrOriginal._movingAnimations);
        _stopDist = followTrOriginal._stopDist;
        _targetPosOffset = followTrOriginal._targetPosOffset;
        control.Animator.AddAnimations(new List<CustomAnimation>(_movingAnimations.NonNullAnimations));
        OnUpdate += Update;
    }

    void Update()
    {
        Vector2 orientationVector = _target.position - ObjControl.transform.position;

        var dirMultiplier = orientationVector.x > 0 ? 1 : -1;
        foreach (var renderer in ObjControl.Renderers)
        {
            renderer.flipX = dirMultiplier == -1;
        }
        Vector2 targetPos = (Vector2)_target.position + _targetPosOffset;
        Vector2 movementVector = targetPos - (Vector2)ObjControl.transform.position;
        if (movementVector.magnitude <= _stopDist)
        {
            if (_movingAnimations.NonNullAnimations.Any(anim => anim.AnimationName == ObjControl.Animator.CurrAnim.AnimationName))
                ObjControl.Animator.EndAnimation(ObjControl.Animator.CurrAnim.AnimationName);
            ObjControl.Animator.ChangeAnim(ObjControl.BehaviourManager.SupportObjData.IdleAnimation.AnimationName);
            return;
        }
        CustomAnimation currDirAnim = _movingAnimations.GetAnim(movementVector.normalized);
        if (ObjControl.Animator.CurrAnim.AnimationName != currDirAnim.AnimationName)
        {
            ObjControl.Animator.ChangeAnim(currDirAnim.AnimationName);
        }
        ObjControl.transform.Translate(movementVector.normalized * Time.deltaTime * _moveSpeed());
    }
}
