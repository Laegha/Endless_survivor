using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BiomeBackgroundManager : MonoBehaviour
{
    //[SerializeField] float _maxDistToTarget;
    //[SerializeField] SpriteRenderer _tilePrefab;
    //[SerializeField] int _tileBaseRenderOffset;
    //[SerializeField] Transform _bgTileHolder;

    //Biome _currBiome;
    //List<BiomeBgLayerHandler> _currentBiomeLayerHandlers = new();

    [SerializeField] BiomeBgTile _tilePrefab;
    [SerializeField] Transform _bgTileHolder;
    Biome _currBiome;
    Vector2 _biomeBgStartPosition;
    List<BiomeBgTile> _currentBiomeBgTiles = new();
    //Add a list of renderers that collapse every layer to make transitions(?+
    private void Start()
    {
        _bgTileHolder.SetParent(Camera.main.transform);
        _bgTileHolder.localPosition = new Vector3(0, 0, -Camera.main.transform.position.z);
    }

    private void Update()
    {
        Vector2 tilePos = new(Mathf.Floor(PlayerControl.pc.transform.position.x), Mathf.Floor(PlayerControl.pc.transform.position.y));
        Biome newBiome = MapManager.mm.GenerationHandler.TileMatrix[tilePos][0].TileBiome;
        if (_currBiome != null && newBiome.BiomeData == _currBiome.BiomeData)
            return;

        List<BiomeBgTile> currentBiomeBgTilesCopy = new(_currentBiomeBgTiles);
        _currBiome = newBiome;
        _biomeBgStartPosition = Camera.main.transform.position;
        foreach (var tile in currentBiomeBgTilesCopy)
        {
            _currentBiomeBgTiles.Remove(tile);
            Destroy(tile.gameObject);//change this for a transition if possible
        }
        
        CreateNewBiomeBgTiles();


        //foreach (var layerHandler in _currentBiomeLayerHandlers)
        //{
        //    layerHandler.UpdateLayer(PlayerControl.pc.transform.position, PlayerControl.pc.PlayerRb.velocity.normalized);
        //}
        //Vector2 tilePos = new(Mathf.Floor(PlayerControl.pc.transform.position.x), Mathf.Floor(PlayerControl.pc.transform.position.y));
        //Biome newBiome = MapManager.mm.GenerationHandler.TileMatrix[tilePos][0].TileBiome;
        //if (_currBiome != null && newBiome.BiomeData == _currBiome.BiomeData)
        //    return;
        //_currBiome = newBiome;
        //foreach (var layerHandler in _currentBiomeLayerHandlers)
        //    layerHandler.RemoveLayer();

        //foreach(var layerInfo in _currBiome.BiomeData.BackgroundData.Layers)
        //{
        //    BiomeBgLayerHandler layerHandler = new(_currBiome.BiomeData.BackgroundData.TileSize, layerInfo, _maxDistToTarget, _tilePrefab, _tileBaseRenderOffset, _bgTileHolder);
        //    _currentBiomeLayerHandlers.Add(layerHandler);
        //}
    }

    private void LateUpdate()
    {
        Vector2 camPos = Camera.main.transform.position;
        Vector2 cameraDisplacement = camPos - _biomeBgStartPosition;
        //_bgTileHolder.transform.position = new Vector2(camPos.x, camPos.y);
        //Update tiles directions with player's rb velocity
        List<BiomeBgTile> currentBiomeBgTilesCopy = new(_currentBiomeBgTiles);
        foreach (var tile in currentBiomeBgTilesCopy)
        {
            tile.ChangeMovingDirection(cameraDisplacement);
        }
    }

    async void CreateNewBiomeBgTiles()
    {
        Vector2 cameraHalfSize = CameraWorldSize.GetCameraHalfExtents(Camera.main);
        List<Vector2> tilesLocalPositions = await Task.Run(() => GetTilesPositions(cameraHalfSize, _currBiome.BiomeData.BackgroundData.TileSize));
        foreach(var tilePos in tilesLocalPositions)
        {
            BiomeBgTile newTile = Instantiate(_tilePrefab);
            newTile.transform.SetParent(_bgTileHolder);
            newTile.transform.localPosition = tilePos;

            newTile.ChangeBiome(_currBiome.BiomeData.BackgroundData.Layers);
            _currentBiomeBgTiles.Add(newTile);
        }
    }


    List<Vector2> GetTilesPositions(Vector2 cameraHalfSize, Vector2 tileSize)
    {
        List<Vector2> tilePositions = new List<Vector2>() { Vector2.zero};
        float remainingHorizontalSpace = cameraHalfSize.x - tileSize.x / 2;
        float remainingVerticalSpace = cameraHalfSize.y - tileSize.y / 2;
        int horizontalTiles = (int)Mathf.Ceil(remainingHorizontalSpace / tileSize.x);
        int verticalTiles = (int)Mathf.Ceil(remainingVerticalSpace / tileSize.y);
        horizontalTiles++;
        verticalTiles+=2;//I don't know why, but if I don't add here, not enough tiles are generated

        for(int x = -horizontalTiles; x <= horizontalTiles; x++)
        {
            for(int y = -verticalTiles; y <= verticalTiles; y++)
            {
                Vector2 tilePos = new(x * tileSize.x, y * tileSize.y);
                if (!tilePositions.Contains(tilePos))
                    tilePositions.Add(tilePos);
            }
        }
        return tilePositions;

        //int horizontalTiles = 1;
        //while(tileSize.x * horizontalTiles < cameraSize.x)
        //{
            //horizontalTiles++;
            //int verticalTiles = 1;
            //while (tileSize.y * verticalTiles < cameraSize.y)
            //{
                //verticalTiles++;
                //Vector2 tilePos = new(tileSize.x * horizontalTiles, tileSize.y * verticalTiles);
                //tilePositions.Add(tilePos);
                //tilePositions.Add(-tilePos);
            //}
        //}
    }
}
