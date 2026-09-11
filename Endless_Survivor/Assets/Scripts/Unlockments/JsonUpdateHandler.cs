using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public static class JsonUpdateHandler
{
    public async static Task CheckForUpdates()
    {
        await UpdateElementJson(UnlockmentsManager.charactersFileName);
        await UpdateElementJson(UnlockmentsManager.weaponsFileName);
        await UpdateElementJson(UnlockmentsManager.passiveItemsFileName);
    }
    async static Task UpdateElementJson(string fileName)
    {
        string streamingJsonPath = Path.Combine(Application.streamingAssetsPath, fileName);
        string streamingJsonData = await Utility.ReadJsonPath(streamingJsonPath);
        List<JsonElementInfo> streamingElements = JsonConvert.DeserializeObject<List<JsonElementInfo>>(streamingJsonData);

        string usableJsonData = await Utility.ReadJson(fileName);
        List<JsonElementInfo> usableElements = JsonConvert.DeserializeObject<List<JsonElementInfo>> (usableJsonData);

        foreach (var element in streamingElements)
        {
            if (usableElements.Any(x => x.fileName == element.fileName))
                continue;
            usableElements.Add(element);
        }

        string newJsonData = JsonConvert.SerializeObject(usableElements);
        Utility.WriteJson(fileName, newJsonData);
    }
}
