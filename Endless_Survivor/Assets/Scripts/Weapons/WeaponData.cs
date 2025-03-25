using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/Weapon", order = 2)]
public class WeaponData : ScriptableObject
{
    public enum IWeaponType
    {
        Proyectile,
        Ray,
        Custom
    }

    [SerializeField] WeaponStats _weaponStats;
    [SerializeField] WeaponStats _statsIncreaseScale;
    [SerializeField] IWeaponType _weaponType = IWeaponType.Proyectile;
    [SerializeReference]WeaponDataTransferInterface _weaponDataTransferInterface;

    [SerializeField] Sprite _weaponDisplaySprite;
    [SerializeField] CustomAnimation _idleAnimation;
    [SerializeField] ChangeOnEndAnimation _attackAnimation;
    
    public Sprite WeaponDisplaySprite { get { return _weaponDisplaySprite; } }
    public WeaponStats WeaponStats { get { return _weaponStats; } }
    public WeaponStats StatsIncreaseScale { get { return _statsIncreaseScale; } }
    public WeaponDataTransferInterface WeaponDataTransferInterface { get { return _weaponDataTransferInterface; } set { _weaponDataTransferInterface = value; } }
    public IWeaponType WeaponType { get { return _weaponType; } set { _weaponType = value; } }
    public List<CustomAnimation> Animations { get { 
            return new List<CustomAnimation>
            {
                _idleAnimation,
                _attackAnimation
            };
        }
    }
}
