using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : ScriptableObject
{
    [SerializeField] WeaponStats _weaponStats;

    public WeaponStats WeaponStats { get { return _weaponStats; } }
}
