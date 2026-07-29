using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(JsonWritersData))]
public class JsonWritersDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        JsonWritersData jsonWritersData = (JsonWritersData)target;
        if (GUILayout.Button("Write PassiveItems"))
        {
            jsonWritersData.WriteItems();
        }
    }
}
