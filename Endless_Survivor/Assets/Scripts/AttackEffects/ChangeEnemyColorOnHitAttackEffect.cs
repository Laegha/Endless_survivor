using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEnemyColorOnHitAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    public override void OnEnemyHit(EnemyControl hitEnemyControl)
    {
        base.OnEnemyHit(hitEnemyControl);
        //hitEnemyControl.Anim
    }
}
