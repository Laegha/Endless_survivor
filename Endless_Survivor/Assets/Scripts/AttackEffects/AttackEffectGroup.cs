using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffectGroup : AttackEffect
{
    [SerializeField] AttackEffect[] _attackEffects;
    public override void OnAttack(Weapon attackingWeapon)
    {
        base.OnAttack(attackingWeapon);
        foreach (var effect in _attackEffects)
        {
            effect.OnAttack(attackingWeapon);
        }
    }
    public override void Update()
    {
        base.Update();
        foreach (var effect in _attackEffects)
        {
            effect.Update();
        }
    }
    public override void OnEnemyHit(EnemyControl hitEnemyControl)
    {
        base.OnEnemyHit(hitEnemyControl);
        foreach(var effect in _attackEffects)
        {
            effect.OnEnemyHit(hitEnemyControl);
        }
    }
}
