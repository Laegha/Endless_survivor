using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEnemyColorOnHitAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    public ChangeEnemyColorOnHitAttackEffect(AttackEffect original, Attack affectedAttack): base(original, affectedAttack)
    {
        base.OnEnemyHit += OnEnemyHit;
    }
    public void ChangeEnemyColor(EnemyControl hitEnemyControl)
    {
        //hitEnemyControl.Anim
    }
}
