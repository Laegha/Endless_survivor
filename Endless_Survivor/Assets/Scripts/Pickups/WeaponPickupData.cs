using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Pickup", menuName = "ScriptableObjects/Pickups/Weapon", order = 2)]
public class WeaponPickupData : PickupData
{
    static readonly int _regularWeaponSpawnChance = 10;
    static readonly int _weaponCopySpawnChance = 20;

    public override void TransferData(GameObject pickupGameObject)
    {
        base.TransferData(pickupGameObject);
        Dictionary<dynamic, int> weaponWeights = new Dictionary<dynamic, int>();
        foreach (var weaponData in GameManager.gm.unlockedWeapons)
        {
            weaponWeights.Add(weaponData, _regularWeaponSpawnChance);//use _weaponCopySpawnChance if the player holds the same weapon
        }
        Roulette weaponRoulette = new Roulette(weaponWeights);
        WeaponData resultWeaponData = weaponRoulette.Spin() as WeaponData;
        CustomAnimation weponIdle = resultWeaponData.Animations.Where(animation => animation.AnimationName == "Idle").ToArray()[0];

        PickupControl control = pickupGameObject.GetComponent<PickupControl>();
        control.Animator.AddAnimations(new List<CustomAnimation> { weponIdle });
        control.Animator.ChangeAnim("Idle");
        pickupGameObject.AddComponent<WeaponPickup>().WeaponData = resultWeaponData;
    }
}
