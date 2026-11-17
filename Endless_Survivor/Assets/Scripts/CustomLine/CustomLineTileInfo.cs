using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CustomLineTileInfo
{
    public Sprite tileSprite;
    [Header("The size of each tile IN PIXELS (one of the axes won't matter depending of wether it's vertical or horizontal)")]
    public Vector2 tileSize;
    [Header("Defines the orientation of the tiles based on the provided size")]
    public bool isVertical;
}
