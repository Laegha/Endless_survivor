using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponPickup : Pickup
{
    WeaponData _weaponData;
    public WeaponData WeaponData {  set { _weaponData = value; } }
    public override void PickUp(PlayerControl playerControl)
    {
        base.PickUp(playerControl);
        GameUIManager.uiManager.WeaponPickupMenu.DisplayMenu(_weaponData);
        Destroy(gameObject);
    }
}
