using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ModBundleBuilder))]
public class ModBundleBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ModBundleBuilder bundleBuilder = target as ModBundleBuilder;
        if (GUILayout.Button("Build all asset bundles"))
        {
            bundleBuilder.BuildAssetBundle();
        }
    }
}
