using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffWeaponsOnHitEnemyStatusEffect : EnemyStatusEffect
{
    new public static bool isUsable => true;
    [Tooltip("Here, wait for external means it will be debuffed when the enemy dies")] [SerializeField] WeaponBuffData _buffData;
    List<WeaponBuffHandler> _activeBuffHandlers = new();

    int _activeStacks;

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
        _activeStacks++;
        var buffedWeapons = PlayerControl.pc.WeaponManager.HeldWeapons;

        WeaponBuffHandler weaponDebuffHandler = new(buffedWeapons, _buffData);
        _activeBuffHandlers.Add(weaponDebuffHandler);
        weaponDebuffHandler.callbackOnEnd += () => _activeBuffHandlers.Remove(weaponDebuffHandler);
    }
    public void DebuffWeapons(EnemyControl placeholder)
    {
        List<WeaponBuffHandler> activeBuffHandlersCopy = new(_activeBuffHandlers);
        foreach (var buffHandler in activeBuffHandlersCopy)
        {
            buffHandler.DebuffWeapons();
        }
    }
}
