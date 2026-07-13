using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndWhenHitStatusEffect : EnemyStatusEffect
{
    new public static bool isUsable => true;
    [SerializeField] int _hitsToEnd;
    int _recievedHits = 0;
    public override void Initialize(EnemyControl affectedEnemyControl, EnemyStatusEffect original)
    {
        base.Initialize(affectedEnemyControl, original);
        var endWhenHitOriginal = original as EndWhenHitStatusEffect;
        _hitsToEnd = endWhenHitOriginal._hitsToEnd;
        EndCondition += () => _recievedHits >= _hitsToEnd;
    }

    public override void EnemyHit()
    {
        base.EnemyHit();
        _recievedHits++;

    }
}
