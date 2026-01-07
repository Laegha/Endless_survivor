using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class Biome
{
    List<Tile> _biomeTiles = new();
    BiomeData _biomeData;

    Wave _lastWave;

    public BiomeData BiomeData {  get { return _biomeData; } }
    public List<Tile> Walls { get { return _biomeTiles.Where(x => x.IsWall).ToList(); } }

    public void SetDirty() => _biomeTiles.RemoveAll(x => x == null);

    //public List<Tile> Side()
    //{
    //    var bottomSide = Walls.Where(x => x.transform.position.x == _biomeTiles[0].transform.position.x).ToList();//bottom side
    //    var leftSide = Walls.Where(x => x.transform.position.y == _biomeTiles[0].transform.position.y).ToList();//left side
    //    var topSide = Walls.Where(x => x.transform.position.x == leftSide.Last().transform.position.x).ToList();
    //    var rightSide = Walls.Where(x => x.transform.position.y == bottomSide.Last().transform.position.y).ToList();

    //}

    public Biome(BiomeData biomeData)
    {
        _biomeData = biomeData;
    }

    public List<Tile> GenerateBiomeTiles(BiomeGenerationInfo generationInfo)
    {
        Vector2 startingPoint = generationInfo.generationStartingPoint;
        List<Tile> generatedTiles = new();
        for (int y = 0; y <= generationInfo.biomeSize.y; y++)
        {
            for (int x = 0; x <= generationInfo.biomeSize.x; x++)
            {
                //if(intersectionTiles.Contains(tile => tile.transform.position == new Vector2(x, y))
                //generate intersection tile with oposite direction to said tile
                Tile generatedTile = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["Tile"].GetComponent<Tile>());
                generatedTile.transform.position = startingPoint + new Vector2(x, y);
                generatedTile.TileBiome = this;
                _biomeTiles.Add(generatedTile);
                generatedTiles.Add(generatedTile);
            }
        }
        return generatedTiles;
    }


}
