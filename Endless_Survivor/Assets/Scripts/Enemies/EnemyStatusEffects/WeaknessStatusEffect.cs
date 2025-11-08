using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaknessStatusEffect : EndByTimeStatusEffect
{
    new public static bool isUsable => true;
    [SerializeField] float _damageMultiplierAddition = .25f;
    public override void Initialize(EnemyControl enemyControl, EnemyStatusEffect original) 
    {
        base.Initialize(enemyControl, original);
        WeaknessStatusEffect originalWeaknessEffect = (WeaknessStatusEffect)original;
        _damageMultiplierAddition = originalWeaknessEffect._damageMultiplierAddition;
    }
    public override void Start()
    {
        base.Start();
        AffectedEnemyControl.EnemyHP.IncomingDamageMultiplier += _damageMultiplierAddition;
    }
    public override void End()
    {
        base.End();
        AffectedEnemyControl.EnemyHP.IncomingDamageMultiplier -= _damageMultiplierAddition;

    }
}
