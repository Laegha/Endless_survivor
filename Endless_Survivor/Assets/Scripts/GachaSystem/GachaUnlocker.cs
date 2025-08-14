using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GachaUnlocker
{
    static string _characterElement = "character";
    static string _weaponElement = "weapon";
    static string _passiveItemElement = "passiveItem";
    static string[] _elementKeys = { _characterElement, _weaponElement, _passiveItemElement };

    public static int gachaCoinCost = 1;
    public static ScriptableObject UnlockRandomElement()
    {
        List<ScriptableObject> unlockableElements = new List<ScriptableObject>();
        unlockableElements.AddRange(UnlockmentsManager.LockedCharacters);
        unlockableElements.AddRange(UnlockmentsManager.LockedWeapons);
        unlockableElements.AddRange(UnlockmentsManager.LockedPassiveItems);
        
        if (unlockableElements.Count == 0)
            return null;

        var unlockedElement = unlockableElements[Random.Range(0, unlockableElements.Count)];
        if(unlockedElement == null)
            return null;
        if(unlockedElement.GetType() == typeof(CharacterData))
        {
            UnlockmentsManager.UnlockCharacter(unlockedElement as CharacterData);
        }
        else if(unlockedElement.GetType() == typeof(WeaponData))
        {
            UnlockmentsManager.UnlockWeapon(unlockedElement as WeaponData);
        }
        else if(unlockedElement.GetType() == typeof(PassiveItemData))
        {
            UnlockmentsManager.UnlockPassiveItem(unlockedElement as PassiveItemData);
        }
        UnlockmentsManager.GachaCoins -= gachaCoinCost;
        return unlockedElement;
    }
}
