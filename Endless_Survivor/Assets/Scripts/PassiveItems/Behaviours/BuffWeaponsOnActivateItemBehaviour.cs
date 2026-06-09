using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffWeaponsOnActivateItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] WeaponStats _statsBuffs;
    [SerializeField] int _maxBuffStacks = 1;
    [SerializeField] WeaponBuffHandler.BuffDurationType _durationType;
    [SerializeField] float _timeDuration = 5f;
    [SerializeField] int _enemyKillsNeeded = 2;
    int _activeStacks = 0;
    List<WeaponBuffHandler> _activeBuffHandlers = new();

    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);

        var buffWeaponsOriginal = original as BuffWeaponsOnActivateItemBehaviour;
        _statsBuffs = new WeaponStats(buffWeaponsOriginal._statsBuffs);
        _maxBuffStacks = buffWeaponsOriginal._maxBuffStacks;

        _durationType = buffWeaponsOriginal._durationType;
        _timeDuration = buffWeaponsOriginal._timeDuration;
        _enemyKillsNeeded = buffWeaponsOriginal._enemyKillsNeeded;
    }

    public override void Activate()
    {
        base.Activate();
        //if too many buffs, don't buff again
        if (_activeStacks >= _maxBuffStacks)
            return;
    }

    void BuffWeapons()
    {
        _activeStacks++;
        var buffedWeapons = PlayerControl.pc.WeaponManager.HeldWeapons;
        foreach (var weapon in buffedWeapons)
            weapon.WeaponStats.TemporalStatIncrease(_statsBuffs, false);

        WeaponBuffHandler weaponDebuffHandler = new(buffedWeapons, _statsBuffs, _durationType, _enemyKillsNeeded, _timeDuration, DecreaseStacks);
        _activeBuffHandlers.Add(weaponDebuffHandler);
        weaponDebuffHandler.callbackOnEnd += () => _activeBuffHandlers.Remove(weaponDebuffHandler);
    }

    void DecreaseStacks()
    {
        _activeStacks--;
    }

    public override void RemoveBehaviour()
    {
        for (int i = 0; i < _activeStacks; i++)
        {
            DecreaseStacks();
        }
    }
}
