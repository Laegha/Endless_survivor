using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/Weapon", order = 2)]
public class WeaponData : ScriptableObject
{
    [SerializeField] WeaponStats _weaponStats;
    [SerializeField] WeaponStats _statsIncreaseScale;
    [SerializeField] AttackEffectData[] _attackEffects;
    [SerializeField] CustomFlags.IWeaponTag[] _weaponTags;
    [SerializeField] CustomFlags.IWeaponPool _weaponPools;
    [SerializeReference] WeaponDataTransferInterface _weaponDataTransferInterface;

    [SerializeField] Sprite _weaponDisplaySprite;
    [SerializeField] CustomAnimation _idleAnimation;
    [SerializeField] ChangeOnEndAnimation _attackAnimation;
    
    public Sprite WeaponDisplaySprite { get { return _weaponDisplaySprite; } }
    public WeaponStats WeaponStats { get { return _weaponStats; } }
    public AttackEffectData[] AttackEffects { get { return _attackEffects; } }
    public WeaponStats StatsIncreaseScale { get { return _statsIncreaseScale; } }
    public WeaponDataTransferInterface WeaponDataTransferInterface { get { return _weaponDataTransferInterface; } set { _weaponDataTransferInterface = value; } }
    public List<CustomAnimation> Animations { get { 
            return new List<CustomAnimation>
            {
                _idleAnimation,
                _attackAnimation
            };
        }
    }
    public CustomFlags.IWeaponTag[] WeaponTags { get { return _weaponTags; } }
    public CustomFlags.IWeaponPool WeaponPools { get { return _weaponPools; } }
}