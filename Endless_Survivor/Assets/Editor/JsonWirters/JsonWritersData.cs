using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "jsonWriter", menuName = "JsonWriter", order = 0)]
public class JsonWritersData : ScriptableObject
{
    string _charactersPath = Path.Combine(Application.streamingAssetsPath, "characters.json");
    [SerializeField] List<CharacterData> _characterBlacklist;
    string _weaponsPath = Path.Combine(Application.streamingAssetsPath, "weapons.json");
    [SerializeField] List<WeaponData> _weaponsBlacklist;
    string _passiveItemsPath = Path.Combine(Application.streamingAssetsPath, "passive_items.json");
    [SerializeField] List<PassiveItemData> _itemBlacklist;
    public void WriteCharacters()
    {
        List<JsonElementInfo> jsonDatas = new();
        CharacterData[] characters = Resources.LoadAll<CharacterData>("");
        foreach (var character in characters)
        {
            if (_characterBlacklist.Contains(character) || character.MenuImage == null)
                continue;
            JsonElementInfo elementInfo = new();
            elementInfo.fileName = character.name;
            elementInfo.isUnlocked = true;
            elementInfo.isNew = false;
            jsonDatas.Add(elementInfo);
        }
        Debug.Log("Wrote characterrs " + jsonDatas.Count);
        var json = JsonConvert.SerializeObject(jsonDatas, Formatting.Indented);
        File.WriteAllText(_charactersPath, json);
    }
    public void WriteWeapons()
    {
        List<JsonElementInfo> jsonDatas = new();
        WeaponData[] weapons = Resources.LoadAll<WeaponData>("");
        foreach (var weapon in weapons)
        {
            if (_weaponsBlacklist.Contains(weapon) || weapon.WeaponDisplaySprite == null)
                continue;
            JsonElementInfo elementInfo = new();
            elementInfo.fileName = weapon.name;
            elementInfo.isUnlocked = true;
            elementInfo.isNew = false;
            jsonDatas.Add(elementInfo);
        }
        var json = JsonConvert.SerializeObject(jsonDatas, Formatting.Indented);
        File.WriteAllText(_weaponsPath, json);
    }
    public void WriteItems()
    {
        List<JsonElementInfo> jsonDatas = new();
        PassiveItemData[] items = Resources.LoadAll<PassiveItemData>("");
        foreach (var item in items)
        {
            if (_itemBlacklist.Contains(item) || item.ItemSprite == null)
                continue;
            JsonElementInfo elementInfo = new();
            elementInfo.fileName = item.name;
            elementInfo.isUnlocked = true;
            elementInfo.isNew = false;
            jsonDatas.Add(elementInfo);
        }
        var json = JsonConvert.SerializeObject(jsonDatas, Formatting.Indented);
        File.WriteAllText(_passiveItemsPath, json);
    }
}
