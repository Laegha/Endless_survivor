using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class VersionUpdateManager : MonoBehaviour
{
    static bool _updateChecked = false;
    readonly static string _currentVersionFileName = "current_version.json";
    readonly static string _versionRecordFileName = "version_record.json";
    readonly static string _versionRecordPath = Path.Combine(Application.streamingAssetsPath, _versionRecordFileName);

    [SerializeField] float _currentVersionNum;
    [SerializeField] VersionUpdateInfo.JsonUpdateMode _currentVersionUpdateMode;
    
    private async void Start()
    {
        if (_updateChecked)
            return;
        _updateChecked = true;
        string currentVersionFilePath = Path.Combine(Application.persistentDataPath, _currentVersionFileName);
        if (!File.Exists(currentVersionFilePath))
        {
            File.WriteAllText(currentVersionFilePath, "-1");
        }
        string currVersionData = File.ReadAllText(currentVersionFilePath);
        float currentVer = float.Parse(currVersionData, System.Globalization.CultureInfo.InvariantCulture);
        string verRecordJson = await Utility.ReadJsonPath(_versionRecordPath);
        List<VersionUpdateInfo> versionRecord = JsonConvert.DeserializeObject<List<VersionUpdateInfo>>(verRecordJson);
        List<VersionUpdateInfo> missingVersions = versionRecord.Where(ver => ver.versionNum > currentVer).ToList();
        if (missingVersions.Count == 0)
            return;

        VersionUpdateInfo.JsonUpdateMode updateMode = VersionUpdateInfo.JsonUpdateMode.Nothing;
        
        foreach (var version in missingVersions)
        {
            if (version.versionUpdateMode == VersionUpdateInfo.JsonUpdateMode.Copy)
            {
                updateMode = VersionUpdateInfo.JsonUpdateMode.Copy;
                break;
            }
            if (version.versionUpdateMode == VersionUpdateInfo.JsonUpdateMode.Difference)
            {
                updateMode = VersionUpdateInfo.JsonUpdateMode.Difference;
            }

        }

        float updatedVer = missingVersions.Last().versionNum;
        string updatedVerData = JsonConvert.SerializeObject(updatedVer);
        Debug.Log(updatedVerData);
        Utility.WriteJson(_currentVersionFileName, updatedVerData);

        if (updateMode == VersionUpdateInfo.JsonUpdateMode.Nothing)
            return;
        if (updateMode == VersionUpdateInfo.JsonUpdateMode.Difference)
        {
            UnlockmentsManager.UpdateDifferencesAll();
            return;
        }
        UnlockmentsManager.CopyAll();
    }

    public async void UpdateVersionRecord()
    {
        string verRecordJson = await Utility.ReadJsonPath(_versionRecordPath);
        List<VersionUpdateInfo> versionRecord = JsonConvert.DeserializeObject<List<VersionUpdateInfo>>(verRecordJson);
        var changedVer = versionRecord.Find(x => x.versionNum == _currentVersionNum);
        if (changedVer == null)
        {
            VersionUpdateInfo addedVer = new();
            addedVer.versionNum = _currentVersionNum;
            addedVer.versionUpdateMode = _currentVersionUpdateMode;
            versionRecord.Add(addedVer);
        }
        else
        {
            changedVer.versionNum = _currentVersionNum;
            changedVer.versionUpdateMode = _currentVersionUpdateMode;
        }
        string newVersionRecordData = JsonConvert.SerializeObject(versionRecord, Formatting.Indented);
        File.WriteAllText(_versionRecordPath, newVersionRecordData);
    }
}
