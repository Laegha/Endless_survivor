using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class WeaponDataTransferInterface
{
    public virtual void TransferData(GameObject weaponObject, WeaponData weaponData, WeaponStats weaponStats)
    {
        Weapon weapon = weaponObject.GetComponent<Weapon>();

        weapon.WeaponStats = new WeaponStats(weaponStats);
        weapon.WeaponData = weaponData;
        AttackEffectsHolder attackEffectsHolder = new();
        attackEffectsHolder.availableEffects = weaponData.AttackEffects.ToList();
        weapon.WeaponAttackEffects = attackEffectsHolder;
        WeaponControl weaponControl = weaponObject.GetComponent<WeaponControl>();
        weaponControl.WeaponAnimator.AddAnimations(weaponData.Animations);

    }
}
