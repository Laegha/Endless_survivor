using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class MapUpdatingHandler 
{
    [SerializeField] Sprite _tilePlaceHolderSprite;
    List<BiomeUpdateInstance> _biomeUpdateInstances = new();
    public void UpdateCurrInstanceTiles()
    {
        if (_biomeUpdateInstances.Count == 0)
            return;
        var updateInstance = _biomeUpdateInstances[0];
        foreach (var tile in updateInstance.tilesToUpdate)
        {
            tile.Renderer.sprite = _tilePlaceHolderSprite;
        }
        GameManager.gm.DelayActionAFrame(UpdateTiles, null);
    }

    void UpdateTiles()
    {
        var updateInstance = _biomeUpdateInstances[0];
        foreach (var tile in updateInstance.tilesToUpdate)
        {
            tile.SetTileGfx();
        }
        MapManager.mm.ActiveBiomes.Add(updateInstance.biome);
        MapManager.mm.LoadedTiles.AddRange(updateInstance.biome.BiomeTiles);
        updateInstance.biome.GenerateDecorations();
        updateInstance.biome.GenerateSupportObjs();
        _biomeUpdateInstances.RemoveAt(0);
    }

    public void AddUpdateInstance(BiomeUpdateInstance biomeUpdateInstance) => _biomeUpdateInstances.Add(biomeUpdateInstance);
}
