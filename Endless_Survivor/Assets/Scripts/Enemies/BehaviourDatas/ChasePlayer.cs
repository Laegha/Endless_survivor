using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class ChasePlayer : EnemyBehaviour
{
    [SerializeField] float _enemySpeed;

    public override void TransferData(EnemyControl enemyControl)
    {
        base.TransferData(enemyControl);

        MoveToTarget moveToTarget = enemyControl.AddComponent<MoveToTarget>();
        moveToTarget.MoveSpeed = _enemySpeed;
        moveToTarget.TargetTag = "Player";
    }
}
