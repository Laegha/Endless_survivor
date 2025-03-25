using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponPickup : Pickup
{
    WeaponData _weaponData;
    static readonly int _regularWeaponSpawnChance = 10;
    static readonly int _weaponCopySpawnChance = 20;
    public WeaponData WeaponData { get { return _weaponData; } }

    private void Start()
    {
        Dictionary<dynamic, int> weaponWeights = new Dictionary<dynamic, int>();
        foreach(var weaponData in GameManager.gm.unlockedWeapons)
        {
            weaponWeights.Add(weaponData, _regularWeaponSpawnChance);//use _weaponCopySpawnChance if the player holds the same weapon
        }
        Roulette weaponRoulette = new Roulette(weaponWeights);
        _weaponData = weaponRoulette.Spin() as WeaponData;
        CustomAnimation weponIdle = _weaponData.Animations.Where(animation => animation.AnimationName == "Idle").ToArray()[0];
        PickupAnimator.AddAnimations(new List<CustomAnimation> { weponIdle });
        PickupAnimator.ChangeAnim("Idle");
    }
    
    public override void PickUp(PlayerControl playerControl)
    {
        base.PickUp(playerControl);
        GameUIManager.uiManager.WeaponPickup.DisplayWeapon(_weaponData);
        Destroy(gameObject);
    }
}
