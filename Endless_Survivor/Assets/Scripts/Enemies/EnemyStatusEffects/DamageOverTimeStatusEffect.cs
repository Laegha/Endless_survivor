using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeStatusEffect : EnemyStatusEffect
{
    new public static bool isUsable => true;
    [SerializeField] DamageInfo _damage;
    [SerializeField] float _damageFrequency = .5f;
    float _damageTimer = 0;
    public override void Initialize(EnemyControl affectedEnemyControl, EnemyStatusEffect original)
    {
        base.Initialize(affectedEnemyControl, original);
        DamageOverTimeStatusEffect damageOverTimeOriginal = original as DamageOverTimeStatusEffect;
        _damage = damageOverTimeOriginal._damage;
        _damageFrequency = damageOverTimeOriginal._damageFrequency;

    }
    public override void Update()
    {
        base.Update();
        _damageTimer += Time.deltaTime;
        if(_damageTimer >= _damageFrequency)
        {
            AffectedEnemyControl.EnemyHP.TakeDamage((int)_damage.CalculatedDamage);
            _damageTimer = 0;
        }
    }
}
