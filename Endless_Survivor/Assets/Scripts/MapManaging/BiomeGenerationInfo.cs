using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeGenerationInfo
{
    public static List<Vector2> allDirections
    {
        get
        {
            Vector2 upRightDir = new Vector2(1, 1);
            Vector2 upLeftDir = new Vector2(-1, 1);
            Vector2 downRightDir = new Vector2(1, -1);
            Vector2 downLeftDir = new Vector2(-1, -1);
            List<Vector2> directions = new List<Vector2>() { upRightDir, upLeftDir, downRightDir, downLeftDir };
            return directions;
        }
    }
    public Vector2 generatorTilePos;
    public Vector2 generationDirection;
    public Vector2 biomeSize;
    public List<Tile> intersectingTiles;
    public Vector2 generationStartingPoint;
    public BiomeGenerationInfo(Vector2 generatorTilePos, Vector2 generationDirection, Vector2 biomeSize, List<Tile> intersectingTiles, Vector2 generationStartingPoint)
    {
        this.generatorTilePos = generatorTilePos;
        this.generationDirection = generationDirection;
        this.biomeSize = biomeSize;
        this.intersectingTiles = intersectingTiles;
        this.generationStartingPoint = generationStartingPoint;
    }
}
