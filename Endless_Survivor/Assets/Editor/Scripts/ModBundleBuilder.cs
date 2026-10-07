using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "bundleBuilder", menuName = "ModBundleBuilder", order = 0)]
public class ModBundleBuilder : ScriptableObject
{
    [SerializeField] BuildTarget _targetPlatform;
    
    public void BuildAssetBundle()
    {
        string bundlePath = "Assets/AssetBundles/" + _targetPlatform;
        if(!Directory.Exists(bundlePath))
            Directory.CreateDirectory(bundlePath);
        BuildPipeline.BuildAssetBundles(bundlePath, BuildAssetBundleOptions.None, _targetPlatform);
    }
}
