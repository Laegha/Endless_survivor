using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Biome
{
    List<Tile> _biomeTiles = new();
    List<Tile> _borderTiles = new();
    BiomeData _biomeData;

    const int floorDecorRenderOffset = -200;

    public List<Tile> BiomeTiles {  get { return _biomeTiles; } }
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

    public void GenerateDecorations()
    {
        foreach(var tile in _biomeTiles)
        {
            if (tile.IsWall || tile.TileDecor != null)
                continue;

            float rand = Random.Range(0, 100f);
            if (rand > _biomeData.DecorationChance)
                continue; 
            GenerateDecor(tile.transform.position);
        }
    }

    void GenerateDecor(Vector2 startPos)
    {
        var placingDecor = MapManager.mm.GetRandomPlaceableMapElement<CustomAnimation>(startPos, _biomeData.FloorDecorations, (Tile tile) => tile.TileDecor != null);
        if (placingDecor == null)
            return ;

        GameObject decor = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["AnimatedObject"], startPos, Quaternion.identity);
        Transform startTileTr = MapManager.mm.GenerationHandler.TileMatrix[startPos][0].transform;
        decor.transform.SetParent(startTileTr);
        
        var decorAnimator = decor.GetComponent<CustomAnimator>();
        decor.GetComponent<RendererSortingByY>().Offset = floorDecorRenderOffset;
        decorAnimator.AddAnimations(new() { placingDecor.Element } );
        decorAnimator.ChangeAnim(placingDecor.Element.AnimationName);

        foreach(Vector2 offset in placingDecor.MapElementSize.ElementOccupyingPositions)
        {
            MapManager.mm.GenerationHandler.TileMatrix[startPos + offset][0].TileDecor = decor;
        }
    }

    public void GenerateSupportObjs()
    {
        foreach (var tile in _biomeTiles)
        {
            if (tile.IsWall || tile.TileSupportObj != null)
                continue;

            float rand = Random.Range(0, 100f);
            if (rand > _biomeData.SupportObjChance)
                continue;
            GenerateSupportObj(tile.transform.position);
        }

    }

    void GenerateSupportObj(Vector2 startPos)
    {
        var placingSupportObj = MapManager.mm.GetRandomPlaceableMapElement(startPos, _biomeData.SupportObjs, (Tile tile) => tile.TileSupportObj != null);
        if (placingSupportObj == null)
            return;

        SupportObjectControl generatedObjControl = Utility.GenerateSupportObj(placingSupportObj.Element, startPos, Quaternion.identity);
        generatedObjControl.transform.SetParent(MapManager.mm.GenerationHandler.TileMatrix[startPos][0].transform);
        foreach (Vector2 offset in placingSupportObj.MapElementSize.ElementOccupyingPositions)
        {
            MapManager.mm.GenerationHandler.TileMatrix[startPos + offset][0].TileSupportObj = generatedObjControl;
        }
    }

    public EnemyData GetRandomAvailableEnemy()
    {
        var possibleEnemies = _biomeData.BiomeEnemies.Where(x => x.RouletteElement.SpawnIntensity <= IntensityManager.im.CurrIntensityLevel).ToList();
        EnemyData spawningEnemy = Utility.GetRouletteElement(possibleEnemies).EnemyData;
        return spawningEnemy;
    }

    public void InitializeBossInvoker()
    {
        Vector2 bossInvokerPosition = _biomeTiles[_biomeTiles.Count / 2].transform.position;
        EnemyInvoker bossInvoker = GameObject.Instantiate(MapManager.mm.GenerationConfig.BossInvokerPrefab, bossInvokerPosition, Quaternion.identity);
        if(_biomeData.UsesBoxCollider)
        {
            BoxCollider2D boxCollider = bossInvoker.AddComponent<BoxCollider2D>();
            boxCollider.size = _biomeData.BoxColliderSize;
            boxCollider.offset = _biomeData.ColliderOffset;
        }
        if (_biomeData.UsesCircleCollider)
        {
            CircleCollider2D circleCollider = bossInvoker.AddComponent<CircleCollider2D>();
            circleCollider.radius = _biomeData.CircleColldierRadius;
            circleCollider.offset = _biomeData.ColliderOffset;
        }
        CustomAnimation invokerAnimation = _biomeData.BossInvokerAnimation;
        bossInvoker.Animator.AddAnimations(new(){ invokerAnimation });
        bossInvoker.Animator.ChangeAnim(invokerAnimation.AnimationName);
        SupportObjectControl invokerSupportObjControl = bossInvoker.GetComponent<SupportObjectControl>();
        foreach (Vector2 tileOffset in MapManager.mm.GenerationConfig.BossInvokerSize.ElementOccupyingPositions)
        {
            var tilesInPos = MapManager.mm.GenerationHandler.TileMatrix[bossInvokerPosition + tileOffset];
            foreach (Tile tile in tilesInPos)
                tile.TileSupportObj = invokerSupportObjControl;
        }

        foreach(var champion in _biomeData.BiomeChampions)
            bossInvoker.AddInvokationEnemy(champion, 1);

        foreach(var boss in _biomeData.BiomeBosses)
            bossInvoker.AddInvokationEnemy(boss, 3);
    }
}
