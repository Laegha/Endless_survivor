using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffWeaponsOnHitEnemyStatusEffect : EnemyStatusEffect
{
    new public static bool isUsable => true;
    [Tooltip("Here, wait for external means it will be debuffed when the enemy dies")] [SerializeField] WeaponBuffData _buffData;

    int _givenStacks = 0;

    public override void Initialize(EnemyControl affectedEnemyControl, EnemyStatusEffect original)
    {
        base.Initialize(affectedEnemyControl, original);
        var buffWeaponsOriginal = original as BuffWeaponsOnHitEnemyStatusEffect;
        _buffData = buffWeaponsOriginal._buffData;

        if (_buffData.DurationType == WeaponBuffHandler.BuffDurationType.WaitForExternal)
            AffectedEnemyControl.EnemyHP.OnDeath += DebuffWeapons;
    }

    public override void EnemyHit()
    {
        base.EnemyHit();
        BuffWeapons();
    }

    void BuffWeapons()
    {
        if (!WeaponBuffsManager.wbm.AddBuffStack(_buffData))
            return;
        _givenStacks++;
    }
    public void DebuffWeapons(EnemyControl placeholder)
    {
        for(int i = 0; i < _givenStacks; i++) 
            WeaponBuffsManager.wbm.RemoveBuffStack(_buffData);
    }
}
