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

    //static async string _charactersJsonData()
    //{
    //string jsonData = await ReadJson(_charactersPath);
    //return jsonData;

    //}

    //static string _charactersJson
    //{
    //    get
    //    {
    //        string jsonData = "";
    //        GameManager.gm.RoutineRunner(ReadJson(_charactersPath, (string param) =>
    //        {
    //            jsonData = param;
    //        }));
    //        return jsonData;
    //    }
    //}
    //static string _weaponsJson
    //{
    //    get
    //    {
    //        string jsonData = "";
    //        GameManager.gm.RoutineRunner(ReadJson(_weaponsPath, (string param) =>
    //        {
    //            jsonData = param;
    //        }));
    //        return jsonData;
    //    }
    //}
    //static string _passiveItemsJson
    //{
    //    get
    //    {
    //        string jsonData = "";
    //        GameManager.gm.RoutineRunner(ReadJson(_passiveItemsPath, (string param) =>
    //        {
    //            jsonData = param;
    //        }));
    //        return jsonData;
    //    }
    //}
    //static string _gachaCoinsJson
    //{
    //    get
    //    {
    //        string jsonData = "";
    //        GameManager.gm.RoutineRunner(ReadJson(_gachaCoinsPath, (string param) =>
    //        {
    //            jsonData = param;
    //        }));
    //        return jsonData;
    //    }
    //}

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
    //public static List<WeaponData> UnlockedWeapons { get { return GetListFromJson<WeaponData>(_weaponsJson, true); } }
    //public static List<WeaponData> LockedWeapons { get { return GetListFromJson<WeaponData>(_weaponsJson, false); } }
    public static async Task<List<WeaponData>> UnlockedWeapons()
    {
        var jsonData = await ReadJson(_weaponsPath);
        var weaponList = GetListFromJson<WeaponData>(jsonData, true);
        return weaponList;
    }
    public static async Task<List<WeaponData>> LockedWeapons()
    {
        var jsonData = await ReadJson(_weaponsPath);
        var weaponList = GetListFromJson<WeaponData>(jsonData, false);
        return weaponList;
    }
    public static void UnlockWeapon(WeaponData unlockedWeapon) => UnlockDataOnJson(unlockedWeapon, _weaponsPath);
    //Characters
    //public static List<CharacterData> UnlockedCharacters { get { return GetListFromJson<CharacterData>(_charactersJson, true); } }
    //public static List<CharacterData> LockedCharacters { get { return GetListFromJson<CharacterData>(_charactersJson, false); } }
    public static async Task<List<CharacterData>> UnlockedCharacters()
    {
        var jsonData = await ReadJson(_charactersPath);
        var characterList = GetListFromJson<CharacterData>(jsonData, true);
        return characterList;
    }
    public static async Task<List<CharacterData>> LockedCharacters()
    {
        var jsonData = await ReadJson(_charactersPath);
        var characterList = GetListFromJson<CharacterData>(jsonData, false);
        return characterList;
    }
    public static void UnlockCharacter(CharacterData unlockedCharacter) => UnlockDataOnJson(unlockedCharacter, _charactersPath);
    //Passive Items
    //public static List<PassiveItemData> UnlockedPassiveItems { get { return GetListFromJson<PassiveItemData>(_passiveItemsJson, true); } }
    //public static List<PassiveItemData> LockedPassiveItems { get { return GetListFromJson<PassiveItemData>(_passiveItemsJson, false); } }
    public static async Task<List<PassiveItemData>> UnlockedPassiveItems()
    {
        var jsonData = await ReadJson(_passiveItemsPath);
        var passiveItemList = GetListFromJson<PassiveItemData>(jsonData, true);
        return passiveItemList;
    }
    public static async Task<List<PassiveItemData>> LockedPassiveItems()
    {
        var jsonData = await ReadJson(_passiveItemsPath);
        var passiveItemList = GetListFromJson<PassiveItemData>(jsonData, false);
        return passiveItemList;
    }
    public static void UnlockPassiveItem(PassiveItemData unlockedPassiveItem) => UnlockDataOnJson(unlockedPassiveItem, _passiveItemsPath);

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
    static List<T> GetListFromJson<T>(string json, bool unlocked) where T : ScriptableObject
    {
        Dictionary<string, bool> jsonDatas = JsonConvert.DeserializeObject<Dictionary<string, bool>>(json);
        T[] allDatas = Resources.LoadAll<T>("");
        List<T> requestedDatas = allDatas.Where(data => jsonDatas.ContainsKey(data.name) && jsonDatas[data.name] == unlocked).ToList();
        return requestedDatas;
    }
    static async void UnlockDataOnJson<T>(T unlockedData, string jsonPath) where T : ScriptableObject
    {
        var json = await ReadJson(jsonPath); 
        Dictionary<string, bool> jsonDatas = JsonConvert.DeserializeObject<Dictionary<string, bool>>(json);
        if (!jsonDatas.ContainsKey(unlockedData.name))
            return;
        jsonDatas[unlockedData.name] = true;
        string newJson = JsonConvert.SerializeObject(jsonDatas, Formatting.Indented);
        File.WriteAllText(jsonPath, newJson);
    }
}
