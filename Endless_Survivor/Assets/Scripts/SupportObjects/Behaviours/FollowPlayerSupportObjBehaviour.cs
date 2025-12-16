using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] CustomAnimation _movingAnimation;
    [SerializeField] Vector2 _playerPosOffset;
    [SerializeField] float _speedLessThanPlayer;
    [SerializeField] float _stopDist;
    Func<float> _moveSpeed;
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var followPlayerOriginal = original as FollowPlayerSupportObjBehaviour;
        _movingAnimation = new (control.Animator, followPlayerOriginal._movingAnimation);
        control.Animator.AddAnimations(new() { _movingAnimation });
        _playerPosOffset = followPlayerOriginal._playerPosOffset;
        _speedLessThanPlayer = followPlayerOriginal._speedLessThanPlayer;
        _stopDist = followPlayerOriginal._stopDist;
        _moveSpeed = () => PlayerControl.pc.PlayerStats.Speed - _speedLessThanPlayer;
        OnUpdate += Update;
    }
    void Update()
    {
        Vector2 orientationVector = PlayerControl.pc.transform.position - ObjControl.transform.position;
        
        var dirMultiplier = orientationVector.x > 0 ? 1 : -1;
        foreach(var renderer in ObjControl.Renderers)
        {
            renderer.flipX = dirMultiplier == -1;
        }
        Vector2 targetPos = (Vector2)PlayerControl.pc.transform.position + _playerPosOffset * dirMultiplier;
        Vector2 movementVector = targetPos - (Vector2)ObjControl.transform.position;
        if (movementVector.magnitude <= _stopDist)
            return;
        if(ObjControl.Animator.CurrAnim.AnimationName != _movingAnimation.AnimationName)
        {
            ObjControl.Animator.ChangeAnim(_movingAnimation.AnimationName);
            return;
        }
        ObjControl.transform.Translate(movementVector.normalized * Time.deltaTime * _moveSpeed());
    }
}
