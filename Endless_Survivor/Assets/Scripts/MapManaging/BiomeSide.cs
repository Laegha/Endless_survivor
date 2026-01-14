using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeSide
{
    public List<Tile> sideTiles;
    public Vector2 sideDirection;
    public Vector2 sideStartPos;
    public Vector2 generationDir 
    {
        get
        {
            return sideDirection + new Vector2(1 - Mathf.Abs(sideDirection.x), 1 - Mathf.Abs(sideDirection.y));
        }
    }

    public BiomeSide(List<Tile> sideTiles, Vector2 sideDirection, Vector2 sideStartPos)
    {
        this.sideTiles = sideTiles;
        this.sideDirection = sideDirection;
        this.sideStartPos = sideStartPos;
    }
}
