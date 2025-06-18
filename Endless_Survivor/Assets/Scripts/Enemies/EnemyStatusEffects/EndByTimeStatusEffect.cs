using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndByTimeStatusEffect : EnemyStatusEffect
{
    new public static bool isUsable => true;
    [SerializeField] float _effectDuration;
    float _timer;

    public override void Initialize(EnemyControl affectedEnemyControl, EnemyStatusEffect original, ConditionHolder endCondition)
    {
        base.Initialize(affectedEnemyControl, original, endCondition);
        var originalEndByTime = (EndByTimeStatusEffect)original;
        _effectDuration = originalEndByTime._effectDuration;
    }

    public override void Update()
    {
        base.Update();
        if(_timer < _effectDuration)
        {
            _timer += Time.deltaTime;
            return;
        }
        EndCondition.value = true;
    }
}
