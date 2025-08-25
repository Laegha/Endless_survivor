using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class GachaUnlocker
{
    static string _characterElement = "character";
    static string _weaponElement = "weapon";
    static string _passiveItemElement = "passiveItem";
    static string[] _elementKeys = { _characterElement, _weaponElement, _passiveItemElement };

    public static int gachaCoinCost = 1;
    public static async Task<ScriptableObject> UnlockRandomElement()
    {
        List<ScriptableObject> unlockableElements = new List<ScriptableObject>();
        var characters = await UnlockmentsManager.LockedCharacters();
        var weapons = await UnlockmentsManager.LockedWeapons();
        var passiveItems = await UnlockmentsManager.LockedPassiveItems();
        unlockableElements.AddRange(characters);
        unlockableElements.AddRange(weapons);
        unlockableElements.AddRange(passiveItems);
        
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
        UnlockmentsManager.AddGachaCoins(-1);
        return unlockedElement;
    }
}
