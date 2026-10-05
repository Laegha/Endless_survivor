using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ModManager : MonoBehaviour
{
    const string modsUrl = "https://api.github.com/repos/Laegha/Endless-Invasion-Mods/contents/Mods";
    const int modListRefreshRateMinutes = 10;
    static ModManager _instance;
    public static ModManager mm { get { return _instance; } }
    //Add path to the mods so that scripts like char select and weapon getter can access and get the scriptable objects(MAYBE NOT)
    //Add .json with all the downloaded mods chars, weapons and items (differents for each)so that each script doesn't have to go through each mod json(NOT A BAD IDEA)
    //UnlockmentsManager should get every T with Resources.Load with the Mods path, then check which ones are unlocked with the .json mentioned on the previous line

    static string _downloadedModsJsonFileName = "downloaded_mods.json";
    static string _lastRefreshJsonFileName = "last_mod_refresh.json";

    static string _allModdedCharsJsonFileName = "modded_characters.json";
    static string _allModdedWeaponsJsonFileName = "modded_weapons.json";
    static string _allModdedItemsJsonFileName = "modded_items.json";
    
    static string _modCharactersJsonFileName = "characters.json";
    static string _modWeaponsJsonFileName = "weapons.json";
    static string _modItemsJsonFileName = "items.json";

    List<ModInfo> _downloadedMods = new();
    List<ModInfo> _availableMods = new();

    public List<ModInfo> AvailableMods { get { return _availableMods; } }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private async void Start()
    {
        UpdateDownloadedMods();
        var refreshResult = await TryRefreshModsList();
        if (refreshResult.Item1)
            return;
        _availableMods = refreshResult.Item2;
    }
    public async Task<(bool, List<ModInfo>)> TryRefreshModsList()
    {
        string lastRefreshData = await Utility.ReadJson(_lastRefreshJsonFileName);
        ModListRefreshInfo lastRefresh = JsonConvert.DeserializeObject<ModListRefreshInfo>(lastRefreshData);
        DateTime now = DateTime.Now;

        TimeSpan timeSpan = now - lastRefresh.lastRefreshTime;
        if (timeSpan.Minutes < modListRefreshRateMinutes)
            return (false, lastRefresh.lastRefreshMods);

        await RefreshModsList();
        ModListRefreshInfo newRefreshInfo = new();
        newRefreshInfo.lastRefreshTime = now;
        newRefreshInfo.lastRefreshMods = new(_availableMods);
        string newTimeData = JsonConvert.SerializeObject(newRefreshInfo, Formatting.Indented);
        Utility.WriteJson(_lastRefreshJsonFileName, newTimeData);
        return (true, new());
    }
    async Task RefreshModsList()
    {
        var modFolders = await FilesDownloader.GetGithubFiles(modsUrl);
        List<ModInfo> mods = new List<ModInfo>();
        foreach (var modFolder in modFolders)
        {
            ModInfo modInfo = new ModInfo();
            var modFiles = await FilesDownloader.GetGithubFiles(Path.Combine(modsUrl, modFolder.name));
            var iconFile = modFiles.Find(x => x.name == "icon.png");
            Sprite iconSprite = await FilesDownloader.DownloadSprite(iconFile.download_url);

            var textFile = modFiles.Find(x => x.name == "display_info.json");
            string modTextJson = await FilesDownloader.DownloadJson(textFile.download_url);
            ModTextInfo textInfo = JsonConvert.DeserializeObject<ModTextInfo>(modTextJson);
            

            modInfo.modIcon = iconSprite;
            modInfo.modTitle = textInfo.title;
            modInfo.modDirectoryName = Utility.GetAvailableIndexedNameInPath(ModInfo.modsDirectoryPath, modFolder.name);
            modInfo.modDescription = textInfo.description;
            modInfo.downloadUrl = modsUrl + "/" + modFolder.name;

            mods.Add(modInfo);
        }
        _availableMods = mods;
    }
    public bool IsModDownloaded(ModInfo mod)
    {
        if(_downloadedMods.Count > 0)
            Debug.Log(_downloadedMods[0].downloadUrl);
        return _downloadedMods.Any(x => x.DirectoryPath == mod.DirectoryPath);
    }
    async void UpdateDownloadedMods()
    {
        string jsonData = await Utility.ReadJson(_downloadedModsJsonFileName);
        _downloadedMods = JsonConvert.DeserializeObject<List<ModInfo>>(jsonData);

    }
    public async Task DownloadMod(ModInfo downloadedModInfo)
    {
        await FilesDownloader.DownloadDirectoryRecursive(downloadedModInfo.downloadUrl, downloadedModInfo.DirectoryPath);
        AddModToJson(downloadedModInfo);
        //add characters, weapons and items to general json
        AddAllModElementsToJson(downloadedModInfo.DirectoryPath);
    }
    void AddModToJson(ModInfo modInfo)
    {
        _downloadedMods.Add(modInfo);
        string jsonData = JsonConvert.SerializeObject(_downloadedMods, Formatting.Indented);
        Utility.WriteJson(_downloadedModsJsonFileName, jsonData);
    }
    void AddAllModElementsToJson(string modDirectoryPath)
    {
        AddModElementsToJson(modDirectoryPath, _modCharactersJsonFileName, _allModdedCharsJsonFileName);
        AddModElementsToJson(modDirectoryPath, _modWeaponsJsonFileName, _allModdedWeaponsJsonFileName);
        AddModElementsToJson(modDirectoryPath, _modItemsJsonFileName, _allModdedItemsJsonFileName);
    }
    async void AddModElementsToJson(string modDirectoryPath, string inFile, string outFile)
    {
        string modElemsPath = Path.Combine(modDirectoryPath, inFile);
        if (!File.Exists(modElemsPath))
            return;
        string modElemsJsonData = await Utility.ReadJsonPath(modElemsPath);
        List<JsonElementInfo> modElems = JsonConvert.DeserializeObject<List<JsonElementInfo>>(modElemsJsonData);

        string allElemsJsonData = await Utility.ReadJson(outFile);
        List<JsonElementInfo> allModdedElems = JsonConvert.DeserializeObject<List<JsonElementInfo>>(allElemsJsonData);
        allModdedElems.AddRange(modElems);

        string updatedModdedElems = JsonConvert.SerializeObject(allModdedElems, Formatting.Indented);
        Utility.WriteJson(outFile, updatedModdedElems);
    }

    public void DeleteMod(ModInfo deletedModInfo)
    {
        //Delete from json
        DeleteModFromJson(deletedModInfo.downloadUrl);
        //remove elements from each json
        DeleteAllModElementsFromJson(deletedModInfo.DirectoryPath);
        //delete files
        DeleteModFiles(deletedModInfo.DirectoryPath);
    }

    void DeleteModFromJson(string modUrl)
    {
        _downloadedMods.RemoveAll(x => x.downloadUrl == modUrl); //this should remove only 1 element
        string jsonData = JsonConvert.SerializeObject(_downloadedMods, Formatting.Indented);
        Utility.WriteJson(_downloadedModsJsonFileName, jsonData);
    }

    void DeleteModFiles(string modDirectoryPath)
    {
        Directory.Delete(modDirectoryPath, true);
    }
    void DeleteAllModElementsFromJson(string modDirectoryPath)
    {
        DeleteModElementsFromJson(modDirectoryPath, _allModdedCharsJsonFileName, _modCharactersJsonFileName);
        DeleteModElementsFromJson(modDirectoryPath, _allModdedWeaponsJsonFileName, _modWeaponsJsonFileName);
        DeleteModElementsFromJson(modDirectoryPath, _allModdedItemsJsonFileName, _modItemsJsonFileName);
    }
    async void DeleteModElementsFromJson(string modDirectoryPath, string inFile, string outFile)
    {
        string modElemsPath = Path.Combine(modDirectoryPath, inFile);
        if (!File.Exists(modElemsPath))
            return;
        string modElemsJsonData = await Utility.ReadJsonPath(modElemsPath);
        List<JsonElementInfo> modElems = JsonConvert.DeserializeObject<List<JsonElementInfo>>(modElemsJsonData);

        string allElemsJsonData = await Utility.ReadJson(outFile);
        List<JsonElementInfo> allModdedElems = JsonConvert.DeserializeObject<List<JsonElementInfo>>(allElemsJsonData);
        foreach(var modElem in modElems)
            allModdedElems.Remove(modElem);

        string updatedModdedElems = JsonConvert.SerializeObject(allModdedElems, Formatting.Indented);
        Utility.WriteJson(outFile, updatedModdedElems);
    }
}