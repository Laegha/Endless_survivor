using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VersionUpdateInfo 
{
    public enum JsonUpdateMode
    {
        Nothing,
        Difference,
        Copy
    }

    [SerializeField] public float versionNum = 1;
    [SerializeField] public JsonUpdateMode versionUpdateMode = JsonUpdateMode.Nothing;
}
