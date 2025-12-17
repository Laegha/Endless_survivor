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

    [SerializeField] CustomFlags.IWeaponTag[] _weaponTags;
    [SerializeField] CustomFlags.IWeaponPool _weaponPools;

    [SerializeReference] WeaponAttackController _defaultAttack;
    [SerializeReference] List<WeaponAttackController> _weaponAttacks = new();
    [SerializeReference] List<WeaponAttackChangeCondition> _attackConditions = new();

    [SerializeField] int _spriteRenderOrderOffset;
    [SerializeField] CustomAnimation _idleAnimation;

    [SerializeField] List<RandomIdleAnimation>_randomIdleAnimations = new List<RandomIdleAnimation>();
    [SerializeField] float _randomIdleAnimChance;
    [SerializeField] float _randomIdleAnimTime;
    
    public Sprite WeaponDisplaySprite { get { return _weaponDisplaySprite; } }
    public string WeaponName { get { return _weaponName; } }
    public WeaponStats WeaponStats { get { return _weaponStats; } }
    public WeaponStats StatsIncreaseScale { get { return new WeaponStats(_rangeIncrease, _attackSpeedIncrease, _damageIncrease, 0, 0, 0); } }
    public CustomFlags.IWeaponTag[] WeaponTags { get { return _weaponTags; } }
    public CustomFlags.IWeaponPool WeaponPools { get { return _weaponPools; } }
    [Tooltip("SET ONLY IN EDITOR!!")]public WeaponAttackController DefaultAttack { get { return _defaultAttack; } set { _defaultAttack = value; } }
    public List<WeaponAttackController> WeaponAttacks { get { return _weaponAttacks; } set { _weaponAttacks = value; } }
    public List<WeaponAttackChangeCondition> AttackConditions { get { return _attackConditions; } }
    public int SpriteRenderOrderOffset { get { return _spriteRenderOrderOffset; } }
    public CustomAnimation IdleAnim { get { return _idleAnimation; } }
    public List<RandomIdleAnimation> RandomIdleAnimations { get { return _randomIdleAnimations; } }
    public float RandomIdleAnimChance { get {   return _randomIdleAnimChance; } }
    public float RandomIdleAnimTime { get { return _randomIdleAnimTime; } }
}