using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EndByTimePlayerEffect : PlayerStatusEffect
{
    new public static int maxStacks => 0;
    [SerializeField] float _effectDuration;
    float _timer;

    public override void Initialize(PlayerStatusEffect original)
    {
        base.Initialize(original);
        var endByTimeOriginal = original as EndByTimePlayerEffect;
        _effectDuration = endByTimeOriginal._effectDuration;
        _timer = _effectDuration;
        EndCondition = ReduceTimer;
    }
    bool ReduceTimer()
    {
        _timer -= Time.deltaTime;
        return _timer <= 0;
    }
}
