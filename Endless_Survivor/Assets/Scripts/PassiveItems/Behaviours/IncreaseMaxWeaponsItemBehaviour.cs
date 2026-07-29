using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseMaxWeaponsItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] List<ListWrapper<WeaponHolderInfo>> _addedWeaponHoldersByStacks;
    [SerializeField] int _addedCharacterDefaultHolders;
    //List<WeaponHolderInfo> addedWeaponHolders --> should know sprite (random from character, or fixed), position (part of the circle or fixed) and visibility (always visible or only if it has weapon)

    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var increaseMaxWeaponsOriginal = original as IncreaseMaxWeaponsItemBehaviour;
        _addedWeaponHoldersByStacks = new(increaseMaxWeaponsOriginal._addedWeaponHoldersByStacks);
        _addedCharacterDefaultHolders = increaseMaxWeaponsOriginal._addedCharacterDefaultHolders;
        behaviourManager.onPicked += AddMaxWeapons;
    }

    void AddMaxWeapons()
    {
        if (_addedWeaponHoldersByStacks.Count > 0)
        {
            int itemHeldCopies = PlayerControl.pc.PassiveItemManager.GetItemCopies(BehaviourManager.PassiveItem.ItemData);
            int maxPositions = _addedWeaponHoldersByStacks.Count;
            int addedHoldersIndex = itemHeldCopies < maxPositions ? itemHeldCopies : itemHeldCopies % maxPositions;
            foreach(var weaponHolder in _addedWeaponHoldersByStacks[addedHoldersIndex].List)
                PlayerControl.pc.WeaponManager.AddWeaponHolder(weaponHolder);

        }
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