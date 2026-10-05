using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModListRefreshInfo
{
    public DateTime lastRefreshTime;
    public List<ModInfo> lastRefreshMods;
}
