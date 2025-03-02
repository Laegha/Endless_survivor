using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/Weapon", order = 2)]
public class WeaponData : ScriptableObject
{
    enum WeaponTypes
    {
        Proyectile,
        Ray,
        Custom
    }
    [SerializeField] WeaponStats _weaponStats;
    WeaponDataTransferInterface _weaponDataTransferInterface;

    public WeaponStats WeaponStats { get { return _weaponStats; } }
    public WeaponDataTransferInterface WeaponDataTransferInterface { get { return _weaponDataTransferInterface; } }
}
