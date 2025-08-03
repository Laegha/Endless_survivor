using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

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

    static string _charactersJson = File.ReadAllText(_charactersPath);
    static string _weaponsJson = File.ReadAllText(_weaponsPath);
    static string _passiveItemsJson = File.ReadAllText(_passiveItemsPath);
    static string _gachaCoinsJson = File.ReadAllText(_gachaCoinsPath);

    public static int GachaCoins
    {
        get
        {
            int collectedCoins = JsonConvert.DeserializeObject<int>(_charactersJson);
            return collectedCoins;
        }
        set
        {
            int collectedCoins = JsonConvert.DeserializeObject<int>(_charactersJson);
            collectedCoins += value;
            string collectedCoinsJson = JsonConvert.SerializeObject(collectedCoins, Formatting.Indented);
            File.WriteAllText(_gachaCoinsPath, collectedCoinsJson);
        }
    }
    static List<T> GetListFromJson<T>(string json, bool unlocked) where T : ScriptableObject
    {
        Dictionary<string, bool> jsonDatas = JsonConvert.DeserializeObject<Dictionary<string, bool>>(json);
        T[] allDatas = Resources.LoadAll<T>("");
        List<T> requestedDatas = allDatas.Where(data => jsonDatas.ContainsKey(data.name) && jsonDatas[data.name] == unlocked).ToList();
        Debug.Log(requestedDatas.Count);
        return requestedDatas;
    }
    static void UnlockDataOnJson<T>(T unlockedData, string json, string jsonPath) where T : ScriptableObject
    {
        Dictionary<string, bool> jsonDatas = JsonConvert.DeserializeObject<Dictionary<string, bool>>(json);
        if (!jsonDatas.ContainsKey(unlockedData.name))
            return;
        jsonDatas[unlockedData.name] = true;
        string newJson = JsonConvert.SerializeObject(jsonDatas, Formatting.Indented);
        File.WriteAllText(jsonPath, newJson);
    }
    //Weapons
    public static List<WeaponData> UnlockedWeapons { get { return GetListFromJson<WeaponData>(_weaponsJson, true); } }
    public static List<WeaponData> LockedWeapons { get { return GetListFromJson<WeaponData>(_weaponsJson, false); } }
    public static void UnlockWeapon(WeaponData unlockedWeapon) => UnlockDataOnJson(unlockedWeapon, _weaponsJson, _weaponsPath);
    //Characters
    public static List<CharacterData> UnlockedCharacters { get { return GetListFromJson<CharacterData>(_charactersJson, true); } }
    public static List<CharacterData> LockedCharacters { get { return GetListFromJson<CharacterData>(_charactersJson, false); } }
    public static void UnlockCharacter(CharacterData unlockedCharacter) => UnlockDataOnJson(unlockedCharacter, _charactersJson, _charactersPath);
    //Passive Items
    public static List<PassiveItemData> UnlockedPassiveItems { get { return GetListFromJson<PassiveItemData>(_passiveItemsJson, true); } }
    public static List<PassiveItemData> LockedPassiveItems { get { return GetListFromJson<PassiveItemData>(_passiveItemsJson, false); } }
    public static void UnlockPassiveItem(PassiveItemData unlockedPassiveItem) => UnlockDataOnJson(unlockedPassiveItem, _passiveItemsJson, _passiveItemsPath);
}
