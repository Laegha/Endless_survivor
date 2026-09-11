using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ModManager : MonoBehaviour
{
    const string modsUrl = "https://api.github.com/repos/Laegha/Endless-Invasion-Mods/contents/Mods";
    static ModManager _instance;
    public static ModManager mm { get { return _instance; } }
    //Add path to the mods so that scripts like char select and weapon getter can access and get the scriptable objects(MAYBE NOT)
    //Add .json with all the downloaded mods chars, weapons and items (differents for each)so that each script doesn't have to go through each mod json(NOT A BAD IDEA)
    //UnlockmentsManager should get every T with Resources.Load with the Mods path, then check which ones are unlocked with the .json mentioned on the previous line

    static string _downloadedModsJsonFileName = "available_gacha_coins.json";

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
        await RefreshModsList();

    }
    public async Task RefreshModsList()
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


            modInfo.ModIcon = iconSprite;
            modInfo.ModTitle = textInfo.title;
            modInfo.ModDirectoryName = Utility.GetAvailableIndexedNameInPath("Assets/Resources/Mods/", modFolder.name);
            modInfo.ModDescription = textInfo.description;
            modInfo.DownloadUrl = Path.Combine(modsUrl, modFolder.name);

            mods.Add(modInfo);
        }
        _availableMods = mods;
    }
    public bool IsModDownloaded(ModInfo mod)
    {
        return _downloadedMods.Any(x => x.DirectoryPath == mod.DirectoryPath);
    }
    public async void DownloadMod(ModInfo downloadedModInfo)
    {
        await FilesDownloader.DownloadDirectoryRecursive(downloadedModInfo.DownloadUrl, downloadedModInfo.DirectoryPath);
        AddModToJson(downloadedModInfo.DownloadUrl, downloadedModInfo.DirectoryPath);
        //add characters, weapons and items to general json
        AddAllModElementsToJson(downloadedModInfo.DirectoryPath);
    }
    async void AddModToJson(string modUrl, string modDirectoryPath)
    {
        string jsonData = await Utility.ReadJson(_downloadedModsJsonFileName);
        List<DownloadedModJsonInfo> downloadedMods = JsonConvert.DeserializeObject<List<DownloadedModJsonInfo>>(jsonData);
        DownloadedModJsonInfo downloadedModInfo = new();
        downloadedModInfo.modUrl = modUrl;
        downloadedModInfo.modDirectoryPath = modDirectoryPath;
        downloadedMods.Add(downloadedModInfo);
        jsonData = JsonConvert.SerializeObject(downloadedMods, Formatting.Indented);
        Utility.WriteJson(_downloadedModsJsonFileName, jsonData);
    }
    void AddAllModElementsToJson(string modDirectoryPath)
    {
        AddModElementsToJson(modDirectoryPath, _allModdedCharsJsonFileName, _modCharactersJsonFileName);
        AddModElementsToJson(modDirectoryPath, _allModdedWeaponsJsonFileName, _modWeaponsJsonFileName);
        AddModElementsToJson(modDirectoryPath, _allModdedItemsJsonFileName, _modItemsJsonFileName);
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
        DeleteModFromJson(deletedModInfo.DownloadUrl);
        //remove elements from each json
        DeleteAllModElementsFromJson(deletedModInfo.DirectoryPath);
        //delete files
        DeleteModFiles(deletedModInfo.DirectoryPath);
    }

    async void DeleteModFromJson(string modUrl)
    {
        string jsonData = await Utility.ReadJson(_downloadedModsJsonFileName);
        List<DownloadedModJsonInfo> downloadedMods = JsonConvert.DeserializeObject<List<DownloadedModJsonInfo>>(jsonData);
        downloadedMods.RemoveAll(x => x.modUrl == modUrl);
        jsonData = JsonConvert.SerializeObject(downloadedMods, Formatting.Indented);
        Utility.WriteJson(_downloadedModsJsonFileName, jsonData);
    }

    void DeleteModFiles(string modDirectoryPath)
    {
        File.Delete(modDirectoryPath);
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