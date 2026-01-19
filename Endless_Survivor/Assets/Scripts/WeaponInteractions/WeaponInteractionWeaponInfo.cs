using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponInteractionWeaponInfo
{
    public WeaponData weaponNeededForTheInteraction;
    public int neededCount;
    public bool isDestroyed;
}
