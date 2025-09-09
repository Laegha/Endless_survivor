using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CustomLineInterface
{
    [Header("The tiles that will form the line in order")]
    public CustomLineAnimatedTile[] tiles;
    [Header("If true, the tiles are generated in the order of the array, if not, at random")]
    public bool followTileOrder;
    [Header("The size of each tile IN PIXELS (one of the axes won't matter depending of wether it's vertical or horizontal)")]
    public Vector2 tileSize;
    [Header("Defines the orientation of the tiles based on the provided size")]
    public bool isVertical;
}
