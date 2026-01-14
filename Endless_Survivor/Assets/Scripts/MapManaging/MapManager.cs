using System.Collections;
using System.Collections.Generic;
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

}
