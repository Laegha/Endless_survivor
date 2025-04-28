using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Pickup", menuName = "ScriptableObjects/Pickups/Weapon", order = 2)]
public class WeaponPickupData : PickupData
{
    static readonly int _regularWeaponSpawnChance = 10;
    static readonly int _weaponCopySpawnChance = 20;

    public override void TransferData(PickupControl pickupControl)
    {
        base.TransferData(pickupControl);
        Dictionary<WeaponData, int> weaponWeights = new Dictionary<WeaponData, int>();
        foreach (var weaponData in GameManager.gm.unlockedWeapons)
        {
            weaponWeights.Add(weaponData, _regularWeaponSpawnChance);//use _weaponCopySpawnChance if the player holds the same weapon
        }
        Roulette<WeaponData> weaponRoulette = new Roulette<WeaponData>(weaponWeights);
        WeaponData resultWeaponData = weaponRoulette.Spin() as WeaponData;
        CustomAnimation weponIdle = resultWeaponData.Animations.Where(animation => animation.AnimationName == "Idle").ToArray()[0];

        PickupControl control = pickupControl.GetComponent<PickupControl>();
        control.Animator.AddAnimations(new List<CustomAnimation> { weponIdle });
        control.Animator.ChangeAnim("Idle");
        pickupControl.gameObject.AddComponent<WeaponPickup>().WeaponData = resultWeaponData;
    }
}
