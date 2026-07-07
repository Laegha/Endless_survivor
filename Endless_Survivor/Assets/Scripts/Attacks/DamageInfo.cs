using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DamageInfo
{
    [System.Flags]
    public enum DamageType
    {
        None = 0,
        Explosive = 1 << 1,
        Fire = 1 << 2,
        Chemical = 1 << 3,
        Ice = 1 << 4,
        Electric = 1 << 5,
        Cutting = 1 << 6,
        Laser = 1 << 7,
        Blunt = 1 << 8

    }
    [SerializeField] float _damageAmmount;
    [SerializeField] DamageType _damageType;

    public float DamageAmmount { get { return _damageAmmount;} set { _damageAmmount = value; } }
    public float CalculatedDamage
    {
        get
        {
            return _damageAmmount * DamageTypesMultipliersManager.DamageTypesManager.GetMultipliers(_damageType);
        }
    }
    public DamageInfo(float damageAmmount, DamageType damageType)
    {
        _damageAmmount = damageAmmount;
        _damageType = damageType;
    }
    public DamageInfo(DamageInfo original)
    {
        _damageAmmount = original._damageAmmount;
        _damageType = original._damageType;
    }
}
