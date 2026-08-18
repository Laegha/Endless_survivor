using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffWeaponsOnActivateItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [Tooltip("Here, wait for external means it will be debuffed when this item is removed")][SerializeField] WeaponBuffData _buffData;

    int _givenStacks = 0;

    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);

        var buffWeaponsOriginal = original as BuffWeaponsOnActivateItemBehaviour;
        _buffData = buffWeaponsOriginal._buffData;

    }

    public override void Activate()
    {
        base.Activate();
        BuffWeapons();
    }

    void BuffWeapons()
    {
        if (!WeaponBuffsManager.wbm.AddBuffStack(_buffData))
            return;
        _givenStacks++;
    }

    public override void RemoveBehaviour()
    {
        if (_buffData.DurationType != WeaponBuffHandler.BuffDurationType.WaitForExternal)
            return;

        for (int i = 0; i < _givenStacks; i++)
            WeaponBuffsManager.wbm.RemoveBuffStack(_buffData);

    }
}
