using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public static class UnlockmentsManager
{
    public static string charactersFileName = "characters.json";
    public static string weaponsFileName = "weapons.json";
    public static string passiveItemsFileName = "passive_items.json";
    public static string _gachaCoinsFileName = "available_gacha_coins.json";


    //static string _charactersJson = File.ReadAllText(_charactersPath);
    //static string _weaponsJson = File.ReadAllText(_weaponsPath);
    //static string _passiveItemsJson = File.ReadAllText(_passiveItemsPath);
    //static string _gachaCoinsJson = File.ReadAllText(_gachaCoinsPath);


    static int _maxCoins = 9999999;

    public static async Task<int> GetGachaCoins()
    {
        var jsonData = await Utility.ReadJson(_gachaCoinsFileName);
        int collectedCoins = JsonConvert.DeserializeObject<int>(jsonData);
        return collectedCoins;
    }
    public static async void AddGachaCoins(int addedCoins)
    {
        var jsonData = await Utility.ReadJson(_gachaCoinsFileName);
        int collectedCoins = JsonConvert.DeserializeObject<int>(jsonData);
        collectedCoins += addedCoins;
        if (collectedCoins > _maxCoins)
            collectedCoins = _maxCoins;
        else if (collectedCoins < 0)
            collectedCoins = 0;
        string collectedCoinsJson = JsonConvert.SerializeObject(collectedCoins, Formatting.Indented);
        Utility.WriteJson(_gachaCoinsFileName, collectedCoinsJson);
    }

    //Weapons
    public static async Task<List<ElementIsNewInfo<WeaponData>>> UnlockedWeapons()
    {
        var jsonData = await Utility.ReadJson(weaponsFileName);
        var weaponList = GetUnlockedElementsFromJsom<WeaponData>(jsonData);
        return weaponList;
    }
    public static async Task<List<WeaponData>> LockedWeapons()
    {
        var jsonData = await Utility.ReadJson(weaponsFileName);
        var weaponList = GetListOfLockedElementsFromJson<WeaponData>(jsonData);
        return weaponList;
    }
    public static void UnlockWeapon(WeaponData unlockedWeapon) => AlterElementInfoOnJson(unlockedWeapon, weaponsFileName, true, true);
    public static void SetNotNewWeapon(WeaponData unlockedWeapon) => AlterElementInfoOnJson(unlockedWeapon, weaponsFileName, true, false);
    //Characters
    public static async Task<List<ElementIsNewInfo<CharacterData>>> UnlockedCharacters()
    {
        var jsonData = await Utility.ReadJson(charactersFileName);
        var characterList = GetUnlockedElementsFromJsom<CharacterData>(jsonData);
        return characterList;
    }
    public static async Task<List<CharacterData>> LockedCharacters()
    {
        var jsonData = await Utility.ReadJson(charactersFileName);
        var characterList = GetListOfLockedElementsFromJson<CharacterData>(jsonData);
        return characterList;
    }
    public static void UnlockCharacter(CharacterData unlockedCharacter) => AlterElementInfoOnJson(unlockedCharacter, charactersFileName, true, true);
    public static void SetNotNewCharacter(CharacterData unlockedCharacter) => AlterElementInfoOnJson(unlockedCharacter, charactersFileName, true, false);
    //Passive Items
    public static async Task<List<ElementIsNewInfo<PassiveItemData>>> UnlockedPassiveItems()
    {
        var jsonData = await Utility.ReadJson(passiveItemsFileName);
        var passiveList = GetUnlockedElementsFromJsom<PassiveItemData>(jsonData);
        return passiveList;
    }
    public static async Task<List<PassiveItemData>> LockedPassiveItems()
    {
        var jsonData = await Utility.ReadJson(passiveItemsFileName);
        var passiveItemList = GetListOfLockedElementsFromJson<PassiveItemData>(jsonData);
        return passiveItemList;
    }
    public static void UnlockPassiveItem(PassiveItemData unlockedPassiveItem) => AlterElementInfoOnJson(unlockedPassiveItem, passiveItemsFileName, true, true);
    public static void SetNotNewPassiveItem(PassiveItemData unlockedPassiveItem) => AlterElementInfoOnJson(unlockedPassiveItem, passiveItemsFileName, true, false);

    static List<ElementIsNewInfo<T>> GetUnlockedElementsFromJsom<T>(string json) where T : ScriptableObject
    {
        List<JsonElementInfo> jsonDatas = JsonConvert.DeserializeObject<List<JsonElementInfo>>(json);
        List<T> allDatas = Resources.LoadAll<T>("").ToList();
        List<ElementIsNewInfo<T>> requestedDatas = new ();
        foreach(var jsonInfo in jsonDatas)
        {
            if (!jsonInfo.isUnlocked)
                continue;
            var foundSO = allDatas.Find(x => x.name == jsonInfo.fileName);
            if(foundSO == null)
            {
                Debug.LogError("There is a " + typeof(T) + " in the json with no corresponding file of name: " + jsonInfo.fileName);
            }
            requestedDatas.Add(new(foundSO, jsonInfo.isNew));
        }
        return requestedDatas;
    }
    static List<T> GetListOfLockedElementsFromJson<T>(string json) where T : ScriptableObject
    {
        List<JsonElementInfo> jsonDatas = JsonConvert.DeserializeObject<List<JsonElementInfo>>(json);
        T[] allDatas = Resources.LoadAll<T>("");
        List<T> requestedDatas = allDatas.Where(data => jsonDatas.Exists(x => x.fileName == data.name) && !jsonDatas.Find(x => x.fileName == data.name).isUnlocked).ToList();
        return requestedDatas;
    }
    static async void AlterElementInfoOnJson<T>(T unlockedData, string fileName, bool unlockmentState, bool isNewState) where T : ScriptableObject
    {
        var json = await Utility.ReadJson(fileName);
        List<JsonElementInfo> jsonDatas = JsonConvert.DeserializeObject<List<JsonElementInfo>>(json);
        var alteredElement = jsonDatas.Find(x => x.fileName == unlockedData.name);
        if (alteredElement == null)
            return;
        alteredElement.isUnlocked = unlockmentState;
        alteredElement.isNew = isNewState;
        string newJson = JsonConvert.SerializeObject(jsonDatas, Formatting.Indented);
        Utility.WriteJson(fileName, newJson);
        GameManager.gm.UnlockedElementHelper.UpdateAll();
    }
}
