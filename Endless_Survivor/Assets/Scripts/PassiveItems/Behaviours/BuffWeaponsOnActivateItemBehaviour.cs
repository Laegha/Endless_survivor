using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffWeaponsOnActivateItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [Tooltip("Here, wait for external means it will be debuffed when this item is removed")][SerializeField] WeaponBuffData _buffData;
    List<WeaponBuffHandler> _activeBuffHandlers = new();

    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);

        var buffWeaponsOriginal = original as BuffWeaponsOnActivateItemBehaviour;

    }

    public override void Activate()
    {
        base.Activate();
        BuffWeapons();
    }

    void BuffWeapons()
    {
        var buffedWeapons = PlayerControl.pc.WeaponManager.HeldWeapons;

        WeaponBuffHandler weaponDebuffHandler = new(buffedWeapons, _buffData);
        _activeBuffHandlers.Add(weaponDebuffHandler);
        weaponDebuffHandler.callbackOnEnd += () => _activeBuffHandlers.Remove(weaponDebuffHandler);
    }

    public override void RemoveBehaviour()
    {
        if (!(_buffData.DurationType == WeaponBuffHandler.BuffDurationType.WaitForExternal))
            return;
        List<WeaponBuffHandler> activeBuffHandlersCopy = new(_activeBuffHandlers);
        foreach (var buffHandler in activeBuffHandlersCopy)
        {
            buffHandler.DebuffWeapons();
        }
    }
}
