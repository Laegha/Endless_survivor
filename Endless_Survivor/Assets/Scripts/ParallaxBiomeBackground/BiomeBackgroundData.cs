using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BiomeBgData", menuName = "ScriptableObjects/Map/Biome Background Data", order = 2)]
public class BiomeBackgroundData : ScriptableObject
{
    [SerializeField] List<BiomeBgLayerInfo> _layers;
    [SerializeField] Vector2 _tileSize;
    public List<BiomeBgLayerInfo> Layers { get { return _layers; } }
    public Vector2 TileSize { get { return _tileSize; } }
}
