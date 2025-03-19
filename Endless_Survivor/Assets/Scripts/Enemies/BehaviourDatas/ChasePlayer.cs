using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class ChasePlayer : EnemyBehaviour
{
    [SerializeField] float _enemySpeed;
    [SerializeField] CustomAnimation _chasingAnimation;

    public ChasePlayer(ChasePlayer original) : base(original)
    {
        _enemySpeed = original._enemySpeed;
        _chasingAnimation = original._chasingAnimation;
    }

    public override void Start(EnemyControl enemyControl)
    {
        base.Start(enemyControl);

        MoveToPlayer moveToPlayer = enemyControl.AddComponent<MoveToPlayer>();
        moveToPlayer.MoveSpeed = _enemySpeed;
        moveToPlayer.EnemyControl = enemyControl;
        enemyControl.CustomAnimator.AddAnimations(new List<CustomAnimation>() { _chasingAnimation });
    }
}
