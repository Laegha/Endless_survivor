using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class BuffWeaponsOnPlayerHitItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => 1;
    [Tooltip("Here, wait for external means it will be debuffed when this item is removed")][SerializeField] WeaponBuffData _buffData;
    [SerializeField] float _chanceOfHappenning = 100f;

    int _givenStacks = 0;

    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var buffWeaponsOriginal = original as BuffWeaponsOnPlayerHitItemBehaviour;
        _buffData = buffWeaponsOriginal._buffData;
        _chanceOfHappenning = buffWeaponsOriginal._chanceOfHappenning;

        behaviourManager.onPlayerDamaged += TryBuffStats;
    }
    void TryBuffStats(int _)
    {
        float rand = Random.Range(0, 100f);
        if (rand > _chanceOfHappenning)
            return;

        if (!WeaponBuffsManager.wbm.AddBuffStack(_buffData))
            return;

        _givenStacks++;
    }

    public override void RemoveBehaviour()
    {
        if (_buffData.DurationType != WeaponBuffHandler.BuffDurationType.WaitForExternal)
            return;

        for (int i = 0; i < _givenStacks; i++)
        {
            WeaponBuffsManager.wbm.RemoveBuffStack(_buffData);
        }
    }
}