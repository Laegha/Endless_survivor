using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

[CreateAssetMenu(fileName = "New Weapon Holder", menuName = "ScriptableObjects/WeaponHolder", order = 10)]
public class UnlockmentsHolder : ScriptableObject
{
    [SerializeField] List<CharacterData> _characters;
    [SerializeField] List<WeaponData>  _weapons;
    
    public List<CharacterData> UnlockedCharacters()
    {
        UnlockmentsInfo unlockmentsData = GameManager.gm.Unlockments;
        List<CharacterData> unlockedCharacters = new List<CharacterData>();
        foreach (var unlockmentState in unlockmentsData.unlocked_characters)
        {
            if (!unlockmentState.Value)
                continue;

            unlockedCharacters.Add(_characters.Find(character => character.name == unlockmentState.Key));
        }
        return unlockedCharacters;
    }

    public List<WeaponData> UnlockedWeapons()
    {
        UnlockmentsInfo unlockmentsData = GameManager.gm.Unlockments;
        List<WeaponData> unlockedWeapons = new List<WeaponData>();
        foreach(var unlockmentState in unlockmentsData.unlocked_weapons)
        {
            if (!unlockmentState.Value)
                continue;

            unlockedWeapons.Add(_weapons.Find(weapon => weapon.name == unlockmentState.Key));
        }
        return unlockedWeapons;
    }
}
