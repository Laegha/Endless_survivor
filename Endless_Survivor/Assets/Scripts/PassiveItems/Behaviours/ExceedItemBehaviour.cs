using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ExceedItemBehaviour : PassiveItemBehaviour
{
    new public static bool isUsable => true;
    [SerializeField] WeaponStats _statBuffOnAttackCoordination;
    [SerializeField] ParticleSystem _onAttackCoordinationParticles;
    [SerializeField] float _particlesDuration = 1;
    Dictionary<Weapon, float> _weaponsInBuffLapse = new();
    readonly float _buffLapseDuration = 0.05f;
    readonly float _buffTimeDuration = 2f;
    Dictionary<Weapon, float> _buffedWeapons = new();
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var exceedOriginal = original as ExceedItemBehaviour;
        _statBuffOnAttackCoordination = new WeaponStats(exceedOriginal._statBuffOnAttackCoordination);
        _onAttackCoordinationParticles = exceedOriginal._onAttackCoordinationParticles;
        _particlesDuration = exceedOriginal._particlesDuration;
        behaviourManager.onAttack += PutWeaponInBuffLapse;
        behaviourManager.onUpdate += CheckForCoordinatedWeapons;
        behaviourManager.onUpdate += DecreaseBuffLapseTime;
        behaviourManager.onUpdate += DecreaseActiveBuffTime;
    }
    void CheckForCoordinatedWeapons()
    {
        if (_weaponsInBuffLapse.Count <= 1)
            return;
        var weaponsInBuffLapse = new List<Weapon>(_weaponsInBuffLapse.Keys);
        foreach (var weapon in weaponsInBuffLapse)
        {
            if (_buffedWeapons.ContainsKey(weapon))
                continue;
            _buffedWeapons.Add(weapon, _buffTimeDuration);
            //Increase weapon stats
            weapon.WeaponStats.TemporalStatIncrease(_statBuffOnAttackCoordination);
            //generate particles
            ParticleConfig exceedParticleConfig = new(_onAttackCoordinationParticles, weapon.transform.position, Quaternion.identity, _particlesDuration, weapon.transform, true, false);
            ParticleManager.pm.SpawnParticles(exceedParticleConfig);
        }
    }
    void PutWeaponInBuffLapse(Weapon weapon)
    {
        if (_buffedWeapons.ContainsKey(weapon))
            return;
        _weaponsInBuffLapse.Add(weapon, _buffLapseDuration);
    }

    void DecreaseBuffLapseTime()
    {
        var weaponsInBuffLapse = new List<Weapon>(_weaponsInBuffLapse.Keys);
        foreach(var weapon in weaponsInBuffLapse)
        {
            _weaponsInBuffLapse[weapon] -= Time.deltaTime;
            if (_weaponsInBuffLapse[weapon]  <= 0)
                _weaponsInBuffLapse.Remove(weapon);
        }
    }
    void DecreaseActiveBuffTime()
    {
        var buffedWeapons = new List<Weapon>(_buffedWeapons.Keys);
        foreach(var weapon in buffedWeapons)
        {
            _buffedWeapons[weapon] -= Time.deltaTime;
            if (_buffedWeapons[weapon] <= 0)
            {
                Debug.Log(weapon + " is NO LONGER buffed by exceed");
                //Decrease stats
                weapon.WeaponStats.TemporalStatIncrease(-_statBuffOnAttackCoordination.Range, -_statBuffOnAttackCoordination.AttackSpeed, -_statBuffOnAttackCoordination.Damage, -_statBuffOnAttackCoordination.Knockback);
                _buffedWeapons.Remove(weapon);
            }
        }
    }
}
