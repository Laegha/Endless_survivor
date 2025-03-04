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
    [SerializeField] IWeaponType _weaponType = IWeaponType.Proyectile;
    [SerializeReference]WeaponDataTransferInterface _weaponDataTransferInterface;

    [InspectorLabel("Animations")]
    [SerializeField] CustomAnimation _idleAnimation;
    [SerializeField] ChangeOnEndAnimation _attackAnimation;

    public WeaponStats WeaponStats { get { return _weaponStats; } }
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
