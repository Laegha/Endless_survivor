using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseMaxWeaponsItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] int _addedMaxWeapons;
    //List<WeaponHolderInfo> addedWeaponHolders --> should know sprite (random from character, or fixed), position (part of the circle or fixed) and visibility (always visible or only if it has weapon)

    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var increaseMaxWeaponsOriginal = original as IncreaseMaxWeaponsItemBehaviour;
        _addedMaxWeapons = increaseMaxWeaponsOriginal._addedMaxWeapons;
        behaviourManager.onPicked += AddMaxWeapons;
    }

    void AddMaxWeapons()
    {
        PlayerControl.pc.WeaponManager.AddMaxWeapons(_addedMaxWeapons);
    }

    public override void RemoveBehaviour()
    {
        PlayerControl.pc.WeaponManager.AddMaxWeapons(-_addedMaxWeapons);
    }
}
