using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseMaxWeaponsItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] List<WeaponHolderInfo> _addedWeaponHolder;
    [SerializeField] int _addedCharacterDefaultHolders;
    //List<WeaponHolderInfo> addedWeaponHolders --> should know sprite (random from character, or fixed), position (part of the circle or fixed) and visibility (always visible or only if it has weapon)

    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var increaseMaxWeaponsOriginal = original as IncreaseMaxWeaponsItemBehaviour;
        _addedWeaponHolder = new(increaseMaxWeaponsOriginal._addedWeaponHolder);
        _addedCharacterDefaultHolders = increaseMaxWeaponsOriginal._addedCharacterDefaultHolders;
        behaviourManager.onPicked += AddMaxWeapons;
    }

    void AddMaxWeapons()
    {
        foreach(var weaponHolder in _addedWeaponHolder)
            PlayerControl.pc.WeaponManager.AddWeaponHolder(weaponHolder);
        var defaultHolders = PlayerControl.pc.CharacterData.DefaultWeaponHolders;
        for (int i = 0; i < _addedCharacterDefaultHolders; i++)
        {
            PlayerControl.pc.WeaponManager.AddWeaponHolder(defaultHolders[Random.Range(0, defaultHolders.Length)]);

        }
    }

    public override void RemoveBehaviour()
    {
        //foreach(var weaponHolder in _addedWeaponHolder)
        //{
            //PlayerControl.pc.WeaponManager.RemoveHolder(weaponHolder);

        //}
    }
}
