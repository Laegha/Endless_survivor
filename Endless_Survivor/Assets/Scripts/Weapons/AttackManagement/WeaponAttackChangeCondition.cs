using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class WeaponAttackChangeCondition
{
    public static bool isUsable => false;
    [SerializeField] string[] _attackIds;
    WeaponAttackManager _weaponAM;
    public WeaponAttackManager WeaponAM { get { return _weaponAM; } }
    public virtual void CopyData(WeaponAttackManager weaponAM, WeaponAttackChangeCondition original)
    {
        _weaponAM = weaponAM;
        _attackIds = original._attackIds;
    }

    public abstract bool IsConditionMet();
    public abstract WeaponAttackController GetAttackController();
}
