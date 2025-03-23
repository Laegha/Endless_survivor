using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[Serializable]
public class ChasePlayer : EnemyBehaviour
{
    [SerializeField] float _enemySpeed;
    [SerializeField] CustomAnimation _chasingAnimation;
    Transform _player;

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);

        ChasePlayer originalChasePlayer = original as ChasePlayer;
        _enemySpeed = originalChasePlayer._enemySpeed;
        _chasingAnimation = originalChasePlayer._chasingAnimation;

        _player = GameObject.FindObjectOfType<PlayerControl>().transform;
        enemyControl.CustomAnimator.AddAnimations(new List<CustomAnimation>() { _chasingAnimation });
    }

    public override void PassiveUpdate()
    {
        base.PassiveUpdate();
        if(!IsActive)
            EnemyControl.BehaviourManager.ActivateBehaviour(this);
    }

    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        if(EnemyControl.CustomAnimator.CurrAnim == null || EnemyControl.CustomAnimator.CurrAnim.AnimationName != _chasingAnimation.AnimationName)
            EnemyControl.CustomAnimator.ChangeAnim(_chasingAnimation.AnimationName);
        Vector2 direction = (_player.position - EnemyControl.transform.position).normalized;
        EnemyControl.Rb.velocity = direction * _enemySpeed;
    }
}
