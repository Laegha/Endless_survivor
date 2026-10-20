using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VersionUpdateManager))]
public class VersionUpdateManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Update Version Record"))
        {
            VersionUpdateManager updateManger = (VersionUpdateManager)target;
            updateManger.UpdateVersionRecord();
        }

    }
}
