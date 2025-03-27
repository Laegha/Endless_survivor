using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponDataTransferInterface
{
    public virtual void TransferData(GameObject weaponObject, WeaponData weaponData, WeaponStats weaponStats)
    {
        Weapon weapon = weaponObject.GetComponent<Weapon>();

        weapon.WeaponStats = weaponStats;
        weapon.WeaponData = weaponData;
        WeaponControl weaponControl = weaponObject.GetComponent<WeaponControl>();
        weaponControl.WeaponAnimator.AddAnimations(weaponData.Animations);

    }
}
