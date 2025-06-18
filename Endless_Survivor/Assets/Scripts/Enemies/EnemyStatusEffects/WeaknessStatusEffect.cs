using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaknessStatusEffect : EnemyStatusEffect
{
    new public static bool isUsable => true;
    [SerializeField] float _damageMultiplierAddition = .25f;
    [SerializeField] float _duration = 5;
    float _timer = 0;
    public override void Initialize(EnemyControl enemyControl, EnemyStatusEffect original, ConditionHolder endCondition) 
    {
        base.Initialize(enemyControl, original, endCondition);
        WeaknessStatusEffect originalWeaknessEffect = (WeaknessStatusEffect)original;
        _damageMultiplierAddition = originalWeaknessEffect._damageMultiplierAddition;
        _duration = originalWeaknessEffect._duration;
    }
    public override void Start()
    {
        base.Start();
        AffectedEnemyControl.EnemyHP.IncomingDamageMultiplier += _damageMultiplierAddition;
    }
    public override void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _duration)
        {
            AffectedEnemyControl.StatusEffectManager.RemoveEffect(this);
        }
    }
    public override void End()
    {
        base.End();
        AffectedEnemyControl.EnemyHP.IncomingDamageMultiplier -= _damageMultiplierAddition;

    }
}
