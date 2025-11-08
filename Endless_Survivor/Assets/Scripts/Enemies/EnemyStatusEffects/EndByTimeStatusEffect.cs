using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndByTimeStatusEffect : EnemyStatusEffect
{
    new public static bool isUsable => false;
    [SerializeField] float _effectDuration;
    float _timer;

    public override void Initialize(EnemyControl affectedEnemyControl, EnemyStatusEffect original)
    {
        base.Initialize(affectedEnemyControl, original);
        var originalEndByTime = (EndByTimeStatusEffect)original;
        _effectDuration = originalEndByTime._effectDuration;
        EndCondition += () => _timer > _effectDuration;
    }

    public override void Update()
    {
        base.Update();
        _timer += Time.deltaTime;
    }
}
