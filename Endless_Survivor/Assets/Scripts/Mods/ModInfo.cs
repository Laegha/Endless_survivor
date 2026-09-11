using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModInfo
{
    Sprite _modIcon;
    string _modTitle;
    string _modDirectoryName;
    string _modDescription;
    string _modCreatorName;
    string _downloadUrl;
    string _directoryPath => "Assets/Resources/Mods/" + _modDirectoryName;

    public Sprite ModIcon { get { return _modIcon; } set { _modIcon = value; } }
    public string ModTitle { get { return _modTitle; } set { _modTitle = value; } }
    public string ModDirectoryName { get { return _modDirectoryName; } set { _modDirectoryName = value; } }
    public string ModDescription { get { return _modDescription; } set { _modDescription = value; } }
    public string ModCreatorName { get { return _modCreatorName; } set { _modCreatorName = value; } }
    public string DownloadUrl { get { return _downloadUrl; } set { _downloadUrl = value; } }
    public string DirectoryPath { get { return _directoryPath; } }
}
