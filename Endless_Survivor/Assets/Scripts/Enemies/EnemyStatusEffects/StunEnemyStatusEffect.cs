using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StunEnemyStatusEffect : EnemyStatusEffect
{
    new public static bool isUsable => true;
    static Dictionary<EnemyControl, int> _stunAmmountPerEnemy = new();
    public override void Start()
    {
        base.Start();
        if(_stunAmmountPerEnemy.ContainsKey(AffectedEnemyControl))
            _stunAmmountPerEnemy[AffectedEnemyControl]++;
        else
            _stunAmmountPerEnemy.Add(AffectedEnemyControl, 1);
        AffectedEnemyControl.RbForcesController.ChangeCurrForce(new(new(0, 0), 0, 10000, ForceMode2D.Impulse, 0));
        AffectedEnemyControl.BehaviourManager.IsStunned = true;
    }
    public override void End()
    {
        base.End();
        _stunAmmountPerEnemy[AffectedEnemyControl]--;
        if (_stunAmmountPerEnemy[AffectedEnemyControl] > 0)
            return;
        _stunAmmountPerEnemy.Remove(AffectedEnemyControl);
        AffectedEnemyControl.BehaviourManager.IsStunned = false;
    }
}
