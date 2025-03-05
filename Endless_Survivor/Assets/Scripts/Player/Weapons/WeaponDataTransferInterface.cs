using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponDataTransferInterface
{
    public virtual void TransferData(GameObject weaponObject, WeaponData weaponData)
    {
        Weapon weapon = weaponObject.GetComponent<Weapon>();
        weapon.WeaponStats = new WeaponStats(weaponData.WeaponStats);
        weapon.WeaponAnimator.AddAnimations(weaponData.Animations); 

    }
}
