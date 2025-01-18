using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/Weapon", order = 2)]
public class WeaponData : ScriptableObject
{
    [SerializeField] WeaponStats _weaponStats;
    [SerializeField] GameObject _weaponPrefab;

    public WeaponStats WeaponStats { get { return _weaponStats; } }
    public GameObject WeaponPrefab { get { return _weaponPrefab; } }
}
