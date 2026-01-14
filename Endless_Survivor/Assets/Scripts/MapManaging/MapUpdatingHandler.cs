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
        _biomeUpdateInstances.RemoveAt(0);
    }

    public void AddUpdateInstance(BiomeUpdateInstance biomeUpdateInstance) => _biomeUpdateInstances.Add(biomeUpdateInstance);
}
