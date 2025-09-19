using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChasePlayerBehaviour : EnemyBehaviour
{
    [SerializeField] float _enemySpeed;
    [SerializeField] CustomAnimation _chasingAnimation;
    Transform _player;

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);

        ChasePlayerBehaviour originalChasePlayer = original as ChasePlayerBehaviour;
        _enemySpeed = originalChasePlayer._enemySpeed;
        _chasingAnimation = originalChasePlayer._chasingAnimation;

        _player = GameObject.FindObjectOfType<PlayerControl>().transform;
        enemyControl.Animator.AddAnimations(new List<CustomAnimation>() { _chasingAnimation });
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
        if (EnemyControl.Animator.CurrAnim == null || EnemyControl.Animator.CurrAnim.AnimationName != _chasingAnimation.AnimationName)
            EnemyControl.Animator.ChangeAnim(_chasingAnimation.AnimationName);
        Vector2 direction = (_player.position - EnemyControl.transform.position).normalized;
        EnemyControl.RbForcesController.ChangeCurrForce(new(direction, _enemySpeed, 0, ForceMode2D.Force));
    }
    public override void KillBehaviour()
    {
        base.KillBehaviour();
        EnemyControl.RbForcesController.Stop();
    }
}
