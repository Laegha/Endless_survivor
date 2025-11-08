using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAfterTimeStatusEffect : EndByTimeStatusEffect
{
    new public static bool isUsable => true;
    [SerializeField] int _damage = 5;

    public override void Initialize(EnemyControl affectedEnemyControl, EnemyStatusEffect original)
    {
        base.Initialize(affectedEnemyControl, original);
        DamageAfterTimeStatusEffect damageAfterTimeOriginal = original as DamageAfterTimeStatusEffect;
        _damage = damageAfterTimeOriginal._damage;
    }

    public override void End()
    {
        base.End();
        AffectedEnemyControl.EnemyHP.TakeDamage(_damage);
    }
}
