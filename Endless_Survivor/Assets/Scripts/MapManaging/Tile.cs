using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] Collider2D _collider;
    [SerializeField] Sprite[] _airSprites;
    SupportObjectControl _tileSupportObj;
    bool _isWall;
    Biome _tileBiome;

    public SpriteRenderer Renderer { get { return _renderer; } }
    public bool IsWall {  get { return _isWall; } }
    public SupportObjectControl TileSupportObj { get { return _tileSupportObj; } }

    public Biome TileBiome {  get { return _tileBiome; } set { _tileBiome = value; } }

    public void SetTileGfx()
    {
        var adyacentTiles = GetAdyacentTiles();
        
        bool isWall = adyacentTiles.Any(tile => tile.Value == null || _airSprites.Contains(tile.Value.Renderer.sprite));
        if(isWall != _isWall && _tileSupportObj != null)
        {
            Destroy(_tileSupportObj.gameObject);
        }
        _isWall = isWall;
        _collider.enabled = _isWall;

        SetSpriteAndDir(adyacentTiles);
    }

    Dictionary<Vector2, Tile> GetAdyacentTiles()
    {
        Dictionary<Vector2, Tile> adyacentTiles = new Dictionary<Vector2, Tile>()
        {
            {new Vector2(0, 1) , null},//up
            {new Vector2(1, 1) , null},//up right
            {new Vector2(1, 0) , null},//right
            {new Vector2(1, -1) , null},//right down
            {new Vector2(0, -1) , null},//down
            {new Vector2(-1, -1) , null},//down left
            {new Vector2(-1, 0) , null},//left
            {new Vector2(-1, 1) , null},//left up
        };
        List<Vector2> directions = adyacentTiles.Keys.ToList();
        foreach (var tileDir in directions)
        {
            var adyacentTilePos = (Vector2)transform.position + tileDir;
            if(MapManager.mm.GenerationHandler.TileMatrix.ContainsKey(adyacentTilePos))
            {
                var tilesInDir = MapManager.mm.GenerationHandler.TileMatrix[adyacentTilePos];
                adyacentTiles[tileDir] = tilesInDir.FirstOrDefault();
            }
        }
        return adyacentTiles;
    }

    void SetSpriteAndDir(Dictionary<Vector2, Tile> adyacentTiles)
    {
        Dictionary<Vector2, Tile> airTiles = adyacentTiles.Where(tile => tile.Value == null || _airSprites.Contains(tile.Value.Renderer.sprite)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        if(airTiles.Count == 0)//is floor
        {
            _renderer.sprite = _tileBiome.BiomeData.FloorTile;
            return;
        }
        if(airTiles.Count == 1)//is an open corner
        {
            _renderer.sprite = GetOpenCornerSprite(airTiles.First().Key);
            return;
        }
        var nonAirTiles = adyacentTiles.Where(tile => tile.Value != null && !_airSprites.Contains(tile.Value.Renderer.sprite)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        if (airTiles.Count > nonAirTiles.Count)//is a closed corner
        {
            var oppositeDirectionTile = adyacentTiles.Where(tile => tile.Value != null && !_airSprites.Contains(tile.Value.Renderer.sprite) && tile.Key.magnitude != 1).FirstOrDefault();
            _renderer.sprite = GetClosedCornerSprite(oppositeDirectionTile.Key * -1);
            return;
        }

        _renderer.sprite = GetWallSprite(airTiles.Keys.Where(dir => dir.magnitude == 1).ToArray()[0]);//the only air tile perpendicular to the wall
    }
    Sprite GetOpenCornerSprite(Vector2 direction)
    {
        if (direction == new Vector2(1, 1))//top right
            return _tileBiome.BiomeData.CornerTopRightOpen ;
        
        if (direction == new Vector2(1, -1))//right bottom
            return _tileBiome.BiomeData.CornerRightBotOpen;
        
        if (direction == new Vector2(-1, -1))//bottom left
            return _tileBiome.BiomeData.CornerBotLeftOpen;
        
        if (direction == new Vector2(-1, 1))//left top
            return _tileBiome.BiomeData.CornerLeftTopOpen;
       
        return null;
    }
    Sprite GetClosedCornerSprite(Vector2 direction)
    {
        if (direction == new Vector2(1, 1))//top right
            return _tileBiome.BiomeData.CornerTopRightClosed;

        if (direction == new Vector2(1, -1))//right bottom
            return _tileBiome.BiomeData.CornerRightBotClosed;

        if (direction == new Vector2(-1, -1))//bottom left
            return _tileBiome.BiomeData.CornerBotLeftClosed;

        if (direction == new Vector2(-1, 1))//left top
            return _tileBiome.BiomeData.CornerLeftTopClosed;

        return null;
    }
    Sprite GetWallSprite(Vector2 direction)
    {
        if (direction == Vector2.up)
            return _tileBiome.BiomeData.TopWallTile;
        
        if (direction == Vector2.right)
            return _tileBiome.BiomeData.RightWallTile;
        
        if (direction == Vector2.down)
            return _tileBiome.BiomeData.BottomWallTile;
        
        if (direction == Vector2.left)
            return _tileBiome.BiomeData.LeftWallTile;
        
        return null;
    }
}