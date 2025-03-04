using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponDataTransferInterface
{
    public virtual void TransferData(GameObject weaponObject, WeaponData weaponData)
    {
        weaponObject.GetComponent<Weapon>().WeaponStats = new WeaponStats(weaponData.WeaponStats);
    }
}
