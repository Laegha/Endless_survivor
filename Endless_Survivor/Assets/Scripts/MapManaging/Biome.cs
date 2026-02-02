using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Biome
{
    List<Tile> _biomeTiles = new();
    List<Tile> _borderTiles = new();
    BiomeData _biomeData;

    Wave _lastWave;

    public BiomeData BiomeData {  get { return _biomeData; } }

    public void SetDirty() => _biomeTiles.RemoveAll(x => x == null);

    //Sides ONLY WORK if _biomeTiles[0] is the bottom left WHICH SHOULD ALWAYS BE THE CASE
    public BiomeSide GetSide(Vector2 direction)
    {
        //List<Tile> sideTiles = _borderTiles.Where(tile => tile.TileDir.x == direction.x || tile.TileDir.y == direction.y).ToList();
        List<Tile> sideTiles = new();
        if(direction ==  Vector2.up)
            sideTiles = _borderTiles.Where(x => x.transform.position.y == _borderTiles.Last().transform.position.y).ToList();
        if(direction ==  Vector2.right)
            sideTiles = _borderTiles.Where(x => x.transform.position.x == _borderTiles.Last().transform.position.x).ToList();
        if(direction ==  Vector2.down)
            sideTiles = _borderTiles.Where(x => x.transform.position.y == _borderTiles.First().transform.position.y).ToList();
        if(direction ==  Vector2.left)
            sideTiles = _borderTiles.Where(x => x.transform.position.x == _borderTiles.First().transform.position.x).ToList();

        if(sideTiles.Count == 0)
            return null;
        Vector2 absDir = new(Mathf.Abs(direction.x), Mathf.Abs(direction.y));
        Vector2 startPosOffset = new(sideTiles[0].transform.position.x * absDir.x, sideTiles[0].transform.position.y * absDir.y);
        Vector2 startPosBase = (Vector2)_biomeTiles[0].transform.position - new Vector2(_biomeTiles[0].transform.position.x * absDir.x, _biomeTiles[0].transform.position.y * absDir.y);
        
        //this should cancel out the unnecessary offset, and only consider what's relevant for the direction (vertical if up/down, horizontal if right/left)
        Vector2 startPos = sideTiles[0].transform.position;
        BiomeSide side = new(sideTiles, direction, startPos);
        return side;
    }
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

                if(y == 0 || x == 0 || y == generationInfo.biomeSize.y || x == generationInfo.biomeSize.x)
                    _borderTiles.Add(generatedTile);

                generatedTiles.Add(generatedTile);
            }
        }
        _biomeTiles.AddRange(generatedTiles);
        return generatedTiles;
    }


}
