using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockedElementsHelper
{
    List<ElementIsNewInfo<CharacterData>> _unlockedCharacters;
    List<CharacterData> _lockedCharacters;
    List<ElementIsNewInfo<WeaponData>> _unlockedWeapons;
    List<WeaponData> _lockedWeapons;
    List<ElementIsNewInfo<PassiveItemData>> _unlockedPassiveItems;
    List<PassiveItemData> _lockedPassiveItems;

    public List<ElementIsNewInfo<CharacterData>> UnlockedCharacters { get { return _unlockedCharacters; } }
    public List<CharacterData> LockedCharacters { get { return _lockedCharacters; } }
    public List<ElementIsNewInfo<WeaponData>> UnlockedWeapons { get { return _unlockedWeapons; } }
    public List<WeaponData> LockedWeapons { get { return _lockedWeapons; } }
    public List<ElementIsNewInfo<PassiveItemData>> UnlockedPassiveItems { get { return _unlockedPassiveItems; } }
    public List<PassiveItemData> LockedPassiveItems { get { return _lockedPassiveItems; } }

    public void UpdateAll()
    {
        UpdateCharacters();
        UpdateWeapons();
        UpdatePassives();
    }
    public async void UpdateCharacters()
    {
        _unlockedCharacters = await UnlockmentsManager.UnlockedCharacters();
        _lockedCharacters = await UnlockmentsManager.LockedCharacters();
    }

    public async void UpdateWeapons()
    {
        _unlockedWeapons = await UnlockmentsManager.UnlockedWeapons();
        _lockedWeapons = await UnlockmentsManager.LockedWeapons();
    }
    public async void UpdatePassives()
    {
        _unlockedPassiveItems = await UnlockmentsManager.UnlockedPassiveItems();
        _lockedPassiveItems = await UnlockmentsManager.LockedPassiveItems();
    }
}
