using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaUnlocker : MonoBehaviour
{
    static string _characterElement = "character";
    static string _weaponElement = "weapon";
    static string _passiveItemElement = "passiveItem";
    static string[] _elementKeys = { _characterElement, _weaponElement, _passiveItemElement };

    void UnlockRandomElement()
    {
        string unlockedElement = _elementKeys[Random.Range(0, _elementKeys.Length)];

        if(unlockedElement == _characterElement)
        {
            var lockedChars = UnlockmentsManager.LockedCharacters;
            var unlockedCharacter = lockedChars[Random.Range(0, lockedChars.Count)];
            UnlockmentsManager.UnlockCharacter(unlockedCharacter);
        }
        else if (unlockedElement == _weaponElement)
        {
            var lockedWeapons = UnlockmentsManager.LockedWeapons;
            var unlockedWeapon = lockedWeapons[Random.Range(0, lockedWeapons.Count)];
            UnlockmentsManager.UnlockWeapon(unlockedWeapon);
        }
        else if (unlockedElement == _passiveItemElement)
        {
            var lockedPassiveItems = UnlockmentsManager.LockedPassiveItems;
            var unlockedPassiveItem = lockedPassiveItems[Random.Range(0, lockedPassiveItems.Count)];
            UnlockmentsManager.UnlockPassiveItem(unlockedPassiveItem);
        }
        UnlockmentsManager.GachaCoins--;
    }
}
