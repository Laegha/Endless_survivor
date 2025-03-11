using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEnemyColorOnHitAttackEffect : AttackEffect
{

    public override void OnEnemyHit(EnemyControl hitEnemyControl)
    {
        base.OnEnemyHit(hitEnemyControl);
        //hitEnemyControl.Anim
    }
}
