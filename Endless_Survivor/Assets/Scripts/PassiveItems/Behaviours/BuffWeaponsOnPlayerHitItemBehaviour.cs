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
    int _activeStacks = 0;
    enum BuffDurationType
    {
        ByWaves,
        ByTime
    }
    [SerializeField] BuffDurationType _durationType;
    [SerializeField] float _timeDuration = 5f;
    [SerializeField] int _wavesDuration = 2;

    Dictionary<WeaponDebuffHandler, int> _debuffHandlersWaveCounter = new();
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var buffWeaponsOriginal = original as BuffWeaponsOnPlayerHitItemBehaviour;
        _statsBuffs = new WeaponStats(buffWeaponsOriginal._statsBuffs);
        _chanceOfHappenning = buffWeaponsOriginal._chanceOfHappenning;
        _maxStacks = buffWeaponsOriginal._maxStacks;
        _gfxChanger = buffWeaponsOriginal._gfxChanger;
        
        _durationType = buffWeaponsOriginal._durationType;
        _timeDuration = buffWeaponsOriginal._timeDuration;
        _wavesDuration = buffWeaponsOriginal._wavesDuration;

        behaviourManager.onPlayerDamaged += TryBuffStats;
        if (_durationType == BuffDurationType.ByWaves)
            behaviourManager.onWaveChanged += DebuffStatsOnWaveChanged;
    }
    void TryBuffStats()
    {
        if(_activeStacks >= _maxStacks)
            return; 
        float rand = Random.Range(0, 100f);
        if (rand > _chanceOfHappenning)
            return;

        _activeStacks++;
        _gfxChanger.ApplyGFX();
        var buffedWeapons = PlayerControl.pc.WeaponManager.HeldWeapons;
        foreach (var weapon in buffedWeapons)
            weapon.WeaponStats.TemporalStatIncrease(_statsBuffs, false);
        
        WeaponDebuffHandler weaponDebuffHandler = new(buffedWeapons, _statsBuffs);

        if (_durationType == BuffDurationType.ByTime)
            GameManager.gm.DelayAction(_timeDuration,() => { weaponDebuffHandler.DebuffWeapons(); DecreaseStacks(); }, null);
        else
            _debuffHandlersWaveCounter.Add(weaponDebuffHandler, 0);
    }
    void DebuffStatsOnWaveChanged()
    {
        var debuffHandlersWaveCounterCopy = new Dictionary<WeaponDebuffHandler, int>(_debuffHandlersWaveCounter);
        foreach(var debuffHandler in debuffHandlersWaveCounterCopy)
        {
            _debuffHandlersWaveCounter[debuffHandler.Key]++;
            if (_debuffHandlersWaveCounter[debuffHandler.Key] >= _wavesDuration)
            {
                debuffHandler.Key.DebuffWeapons();
                DecreaseStacks();
                _debuffHandlersWaveCounter.Remove(debuffHandler.Key);
            }
        }

    }
    void DecreaseStacks()
    {
        _activeStacks--;
        if (_activeStacks <= 0)
            _gfxChanger.UnApplyGFX();
    }
}