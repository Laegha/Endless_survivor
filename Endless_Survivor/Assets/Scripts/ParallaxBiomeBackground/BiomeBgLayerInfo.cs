using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BiomeBgLayerInfo
{
    [SerializeField] Sprite _layerTileSprite;
    [SerializeField] float _layerSpeed;
    [Tooltip("Decreases the render priority of the layer")][SerializeField] int _layerDepth;
    public Sprite LayerTileSprite { get { return _layerTileSprite; } }
    public float LayerSpeed { get { return _layerSpeed; } }
    public int LayerDepth { get { return _layerDepth; } }
}
