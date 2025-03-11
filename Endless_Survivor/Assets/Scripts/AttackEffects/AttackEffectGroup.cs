using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffectGroup : AttackEffect
{
    [SerializeField] AttackEffect[] _attackEffects;
    public override void Start()
    {
        base.Start();
        foreach (var effect in _attackEffects)
        {
            effect.Start();
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
