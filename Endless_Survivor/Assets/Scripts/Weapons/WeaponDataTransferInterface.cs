using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
        if(weaponData.RandomIdleAnimations.Count > 0 )
        {
            List<CustomAnimation> randomIdleAnims = new List<CustomAnimation>(weaponData.RandomIdleAnimations);
            weaponControl.WeaponAnimator.AddAnimations(randomIdleAnims);
            var idleAnimator = weaponControl.AddComponent<RandomIdleAnimator>();
            idleAnimator.SetData(weaponControl.WeaponAnimator,weaponData.RandomIdleAnimChance, weaponData.RandomIdleAnimTime, weaponData.RandomIdleAnimations);
        }

    }
}
