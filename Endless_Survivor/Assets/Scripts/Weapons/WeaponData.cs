using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/Weapon", order = 2)]
public class WeaponData : ScriptableObject
{
    [SerializeField] Sprite _weaponDisplaySprite;
    [SerializeField] string _weaponName;

    [SerializeField] WeaponStats _weaponStats;
    [SerializeField] float _rangeIncrease;
    [SerializeField] float _attackSpeedIncrease;
    [SerializeField] float _damageIncrease;
    [SerializeField] AttackEffectData[] _attackEffects;
    [SerializeField] CustomFlags.IWeaponTag[] _weaponTags;
    [SerializeField] CustomFlags.IWeaponPool _weaponPools;
    [SerializeReference] WeaponDataTransferInterface _weaponDataTransferInterface;

    [SerializeField] CustomAnimation _idleAnimation;
    [SerializeField] ChangeOnEndAnimation _attackAnimation;

    [SerializeField] List<RandomIdleAnimation>_randomIdleAnimations = new List<RandomIdleAnimation>();
    [SerializeField] float _randomIdleAnimChance;
    [SerializeField] float _randomIdleAnimTime;
    
    public Sprite WeaponDisplaySprite { get { return _weaponDisplaySprite; } }
    public string WeaponName { get { return _weaponName; } }
    public WeaponStats WeaponStats { get { return _weaponStats; } }
    public WeaponStats StatsIncreaseScale { get { return new WeaponStats(_rangeIncrease, _attackSpeedIncrease, new DamageInfo(_damageIncrease,0), 0, 0, 0); } }
    public AttackEffectData[] AttackEffects { get { return _attackEffects; } }
    public CustomFlags.IWeaponTag[] WeaponTags { get { return _weaponTags; } }
    public CustomFlags.IWeaponPool WeaponPools { get { return _weaponPools; } }
    public WeaponDataTransferInterface WeaponDataTransferInterface { get { return _weaponDataTransferInterface; } set { _weaponDataTransferInterface = value; } }
    public List<CustomAnimation> Animations { get { 
            return new List<CustomAnimation>
            {
                _idleAnimation,
                _attackAnimation
            };
        }
    }
    public List<RandomIdleAnimation> RandomIdleAnimations { get { return _randomIdleAnimations; } }
    public float RandomIdleAnimChance { get {   return _randomIdleAnimChance; } }
    public float RandomIdleAnimTime { get { return _randomIdleAnimTime; } }
}