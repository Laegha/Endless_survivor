using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "jsonWriter", menuName = "JsonWriter", order = 0)]
public class JsonWritersData : ScriptableObject
{
    string _passiveItemsPath = Path.Combine(Application.streamingAssetsPath, "passive_items.json");
    [SerializeField] List<PassiveItemData> _itemBlacklist;
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
