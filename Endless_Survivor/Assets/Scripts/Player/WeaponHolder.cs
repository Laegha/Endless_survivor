using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder
{
    public WeaponHolderInfo holderInfo;
    public Transform handTransform;
    public WeaponAttackManager holdingWeapon;

    public WeaponHolder(WeaponHolderInfo holderInfo, Transform handTransform)
    {
        this.holderInfo = holderInfo;
        this.handTransform = handTransform;
    }
}
