using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.Rendering.DebugUI;

public static class UnlockmentsManager
{
    static string _charactersFileName = "characters.json";
    static string _weaponsFileName = "weapons.json";
    static string _passiveItemsFileName = "passive_items.json";
    static string _gachaCoinsFileName = "available_gacha_coins.json";

    static string _charactersPath = Path.Combine(Application.streamingAssetsPath, _charactersFileName);
    static string _weaponsPath = Path.Combine(Application.streamingAssetsPath, _weaponsFileName);
    static string _passiveItemsPath = Path.Combine(Application.streamingAssetsPath, _passiveItemsFileName);
    static string _gachaCoinsPath = Path.Combine(Application.streamingAssetsPath, _gachaCoinsFileName);

    //static string _charactersJson = File.ReadAllText(_charactersPath);
    //static string _weaponsJson = File.ReadAllText(_weaponsPath);
    //static string _passiveItemsJson = File.ReadAllText(_passiveItemsPath);
    //static string _gachaCoinsJson = File.ReadAllText(_gachaCoinsPath);


    static int _maxCoins = 9999999;

    public static async void GetGachaCoins(Action<int> callback)
    {
        var jsonData = await ReadJson(_gachaCoinsPath);
        int collectedCoins = JsonConvert.DeserializeObject<int>(jsonData);
        callback?.Invoke(collectedCoins);

    }
    public static async void AddGachaCoins(int addedCoins)
    {
        var jsonData = await ReadJson(_gachaCoinsPath);
        int collectedCoins = JsonConvert.DeserializeObject<int>(jsonData);
        collectedCoins += addedCoins;
        if (collectedCoins > _maxCoins)
            collectedCoins = _maxCoins;
        else if (collectedCoins < 0)
            collectedCoins = 0;
        string collectedCoinsJson = JsonConvert.SerializeObject(collectedCoins, Formatting.Indented);
        File.WriteAllText(_gachaCoinsPath, collectedCoinsJson);
    }

    //Weapons
    public static async Task<List<ElementIsNewInfo<WeaponData>>> UnlockedWeapons()
    {
        var jsonData = await ReadJson(_weaponsPath);
        var weaponList = GetUnlockedElementsFromJsom<WeaponData>(jsonData);
        return weaponList;
    }
    public static async Task<List<WeaponData>> LockedWeapons()
    {
        var jsonData = await ReadJson(_weaponsPath);
        var weaponList = GetListOfLockedElementsFromJson<WeaponData>(jsonData);
        return weaponList;
    }
    public static void UnlockWeapon(WeaponData unlockedWeapon) => AlterElementInfoOnJson(unlockedWeapon, _weaponsPath, true, true);
    public static void SetNotNewWeapon(WeaponData unlockedWeapon) => AlterElementInfoOnJson(unlockedWeapon, _weaponsPath, true, false);
    //Characters
    public static async Task<List<ElementIsNewInfo<CharacterData>>> UnlockedCharacters()
    {
        var jsonData = await ReadJson(_charactersPath);
        var characterList = GetUnlockedElementsFromJsom<CharacterData>(jsonData);
        return characterList;
    }
    public static async Task<List<CharacterData>> LockedCharacters()
    {
        var jsonData = await ReadJson(_charactersPath);
        var characterList = GetListOfLockedElementsFromJson<CharacterData>(jsonData);
        return characterList;
    }
    public static void UnlockCharacter(CharacterData unlockedCharacter) => AlterElementInfoOnJson(unlockedCharacter, _charactersPath, true, true);
    public static void SetNotNewCharacter(CharacterData unlockedCharacter) => AlterElementInfoOnJson(unlockedCharacter, _charactersPath, true, false);
    //Passive Items
    public static async Task<List<ElementIsNewInfo<PassiveItemData>>> UnlockedPassiveItems()
    {
        var jsonData = await ReadJson(_passiveItemsPath);
        var passiveList = GetUnlockedElementsFromJsom<PassiveItemData>(jsonData);
        return passiveList;
    }
    public static async Task<List<PassiveItemData>> LockedPassiveItems()
    {
        var jsonData = await ReadJson(_passiveItemsPath);
        var passiveItemList = GetListOfLockedElementsFromJson<PassiveItemData>(jsonData);
        return passiveItemList;
    }
    public static void UnlockPassiveItem(PassiveItemData unlockedPassiveItem) => AlterElementInfoOnJson(unlockedPassiveItem, _passiveItemsPath, true, true);
    public static void SetNotNewPassiveItem(PassiveItemData unlockedPassiveItem) => AlterElementInfoOnJson(unlockedPassiveItem, _passiveItemsPath, true, false);

    static async Task<string> ReadJson(string path)
    {
        string jsonData = "";

        if (path.StartsWith("jar") || path.StartsWith("http"))
        {
            UnityWebRequest request = UnityWebRequest.Get(path);
            //await request.SendWebRequest();
            var operation = request.SendWebRequest();
            while(!operation.isDone)
            {
                await Task.Yield();

            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                jsonData = request.downloadHandler.text;
            }
        }
        else
        {
            jsonData = File.ReadAllText(path);
        }
        return jsonData;
    }
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
    static async void AlterElementInfoOnJson<T>(T unlockedData, string jsonPath, bool unlockmentState, bool isNewState) where T : ScriptableObject
    {
        var json = await ReadJson(jsonPath);
        List<JsonElementInfo> jsonDatas = JsonConvert.DeserializeObject<List<JsonElementInfo>>(json);
        var alteredElement = jsonDatas.Find(x => x.fileName == unlockedData.name);
        if (alteredElement == null)
            return;
        alteredElement.isUnlocked = unlockmentState;
        alteredElement.isNew = isNewState;
        string newJson = JsonConvert.SerializeObject(jsonDatas, Formatting.Indented);
        File.WriteAllText(jsonPath, newJson);
    }
}
