using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DamageInfo
{
    public enum DamageType
    {
        None,
        Normal,
        Explosive,
        Fire,
        Poison,
        Ice,
        Electric,
        Cutting,
        Laser

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
}
