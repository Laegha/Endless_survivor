using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffWeaponsOnPlayerHitItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] WeaponStats _statsBuffs;
    [SerializeField] float _chanceOfHappenning = 100f;
    [SerializeField] int _maxStacks = 1;
    //ADD GFX CHANGE TO THE PLAYER!!!!
    [SerializeField] PlayerGFXChanger _gfxChanger;
    [SerializeField] SFXInfo _onBuffSFX;
    int _activeStacks = 0;
    [SerializeField] WeaponBuffHandler.BuffDurationType _durationType;
    [SerializeField] float _timeDuration = 5f;
    [SerializeField] int _enemyKillsNeeded = 2;
    List<WeaponBuffHandler> _activeBuffHandlers = new();

    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var buffWeaponsOriginal = original as BuffWeaponsOnPlayerHitItemBehaviour;
        _statsBuffs = new WeaponStats(buffWeaponsOriginal._statsBuffs);
        _chanceOfHappenning = buffWeaponsOriginal._chanceOfHappenning;
        _maxStacks = buffWeaponsOriginal._maxStacks;
        _gfxChanger = buffWeaponsOriginal._gfxChanger;
        _onBuffSFX = buffWeaponsOriginal._onBuffSFX;

        _durationType = buffWeaponsOriginal._durationType;
        _timeDuration = buffWeaponsOriginal._timeDuration;
        _enemyKillsNeeded = buffWeaponsOriginal._enemyKillsNeeded;

        behaviourManager.onPlayerDamaged += TryBuffStats;
    }
    void TryBuffStats(int _)
    {
        if(_activeStacks >= _maxStacks)
            return; 
        float rand = Random.Range(0, 100f);
        if (rand > _chanceOfHappenning)
            return;

        _activeStacks++;
        _gfxChanger.ApplyGFX();
        SoundFXManager.sm.PlaySfx(_onBuffSFX, PlayerControl.pc.transform.position);
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
        if (_activeStacks <= 0)
            _gfxChanger.UnApplyGFX();
    }

    public override void RemoveBehaviour()
    {
        for(int i = 0; i < _activeStacks; i++)
        {
            DecreaseStacks();
        }
    }
}