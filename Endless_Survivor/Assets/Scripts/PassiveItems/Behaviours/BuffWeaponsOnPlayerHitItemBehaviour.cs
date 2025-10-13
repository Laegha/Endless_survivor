using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffWeaponsOnPlayerHitItemBehaviour : PassiveItemBehaviour
{
    new public static bool isUsable => true;
    [SerializeField] WeaponStats _statsBuffs;
    [SerializeField] float _chanceOfHappenning = 100f;
    //ADD GFX CHANGE TO THE PLAYER!!!!
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
        
        _durationType = buffWeaponsOriginal._durationType;
        _timeDuration = buffWeaponsOriginal._timeDuration;
        _wavesDuration = buffWeaponsOriginal._wavesDuration;

        behaviourManager.onPlayerDamaged += TryBuffStats;
        if (_durationType == BuffDurationType.ByWaves)
            behaviourManager.onWaveChanged += DebuffStatsOnWaveChanged;
    }
    void TryBuffStats()
    {
        float rand = Random.Range(0, 100f);
        if (rand > _chanceOfHappenning)
            return;
        var buffedWeapons = PlayerControl.pc.WeaponManager.HeldWeapons;
        foreach (var weapon in buffedWeapons)
            weapon.WeaponStats.TemporalStatIncrease(_statsBuffs, false);
        
        WeaponDebuffHandler weaponDebuffHandler = new(buffedWeapons, _statsBuffs);

        if (_durationType == BuffDurationType.ByTime)
            GameManager.gm.DelayAction(_timeDuration, weaponDebuffHandler.DebuffWeapons, null);
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
                _debuffHandlersWaveCounter.Remove(debuffHandler.Key);
            }
        }
    }
}