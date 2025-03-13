using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using System;

[CreateAssetMenu(fileName = "New PrefabHolder", menuName = "ScriptableObjects/PrefabHolder", order = 0)]
public class PrefabHolder : ScriptableObject
{
    [SerializeField] SerializedDictionary<string, GameObject> _prefabs;

    public SerializedDictionary<string, GameObject> Prefabs { get { return _prefabs; } }
}
