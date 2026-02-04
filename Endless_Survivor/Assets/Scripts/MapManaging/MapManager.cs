using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] MapGenerationConfig _generationConfig;
    MapGenerationHandler _generationHandler = new();
    [SerializeField]MapUpdatingHandler _updatingHandler;

    List<Biome> _activeBiomes = new();

    public MapGenerationConfig GenerationConfig {  get { return _generationConfig; } }
    public MapGenerationHandler GenerationHandler {  get { return _generationHandler; } }
    public List<Biome> ActiveBiomes { get { return _activeBiomes; } }
    static MapManager instance;
    public static MapManager mm
    {
        get
        {
            return instance;
        }
    }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        
    }

    public void GenerateBiome()
    {
        var updateInstance = _generationHandler.GenerateBiome();
        _updatingHandler.AddUpdateInstance(updateInstance);
    }
    public void UpdateBiome()
    {
        _updatingHandler.UpdateCurrInstanceTiles();
    }

    public MapElementInfo<T> GetRandomPlaceableMapElement<T>(Vector2 startPos, List<RouletteElementChance<MapElementInfo<T>>> possibleElements, Func<Tile, bool> additionalPlacingCondition)
    {
        MapElementInfo<T> placeableElement = null;
        if(additionalPlacingCondition == null)
            additionalPlacingCondition = (Tile tile) => false;

        bool found = false;
        while (!found)
        {
            if (possibleElements.Count == 0)//there are no fitting elements (all possible need a tile that isn't available)
            {
                Debug.Log("NO ITEM FOUNSD");
                found = true; 
                break;

            }

            MapElementInfo<T> elemInfo = Utility.GetRouletteElement(possibleElements);
            List<Vector2> occupyingTilesOffsets = elemInfo.MapElementSize.ElementOccupyingPositions;
            List<Vector2> banningOffsets = new();

            foreach (Vector2 offset in occupyingTilesOffsets)
            {
                Vector2 tilePos = startPos + offset;
                if (!MapManager.mm.GenerationHandler.TileMatrix.ContainsKey(tilePos))
                {
                    banningOffsets.Add(offset);
                    continue;
                }

                var tilesInOffset = MapManager.mm.GenerationHandler.TileMatrix[tilePos];

                if (tilesInOffset.Count > 1 || additionalPlacingCondition(tilesInOffset[0]) || tilesInOffset[0].IsWall)
                    banningOffsets.Add(offset);

            }

            if (banningOffsets.Count == 0)//a fitting element was found
            {
                placeableElement = elemInfo;

                Debug.Log("ITEM FOUND: " + placeableElement.Element);
                found = true;
                break;

            }

            List<RouletteElementChance<MapElementInfo<T>>> removedDecors = new(possibleElements.Where(x => x.Element.MapElementSize.ElementOccupyingPositions.Intersect(banningOffsets).Count() > 0));//all decors that need a tile that is unavailable
            foreach (var removedDecor in removedDecors)
            {
                possibleElements.Remove(removedDecor);
            }
        }
        return placeableElement;
    }
}
