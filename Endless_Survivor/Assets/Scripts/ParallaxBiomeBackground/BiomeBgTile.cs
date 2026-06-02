using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeBgTile : MonoBehaviour
{
    List<BiomeBgTileLayer> _activeLayers = new();
    [SerializeField] SpriteRenderer _layerPrefab;
    [SerializeField] Material _layerMaterialOriginal;
    [SerializeField] int _layerBaseOffset;

    public void ChangeBiome(List<BiomeBgLayerInfo> newLayersInfos)
    {
        foreach(var layer in _activeLayers)
        {
            _activeLayers.Remove(layer);
            Destroy(layer.renderer.gameObject);
        }

        foreach (var layerInfo in newLayersInfos)
        {
            SpriteRenderer layerRenderer = Instantiate(_layerPrefab);
            layerRenderer.transform.SetParent(transform);
            layerRenderer.transform.localPosition = Vector2.zero;
            layerRenderer.GetComponent<RendererSortingByY>().Offset = _layerBaseOffset - layerInfo.LayerDepth;
            Material layerMaterialCopy = new(_layerMaterialOriginal);

            layerRenderer.material = layerMaterialCopy;
            layerRenderer.sprite = layerInfo.LayerTileSprite;
            
            BiomeBgTileLayer newLayer = new(layerMaterialCopy, layerRenderer, layerInfo);
            _activeLayers.Add(newLayer);
        }

    }

    public void ChangeMovingDirection(Vector2 cameraDisplacement)
    {
        foreach(var layer in _activeLayers)
        {
            Vector2 layerNewOffset = cameraDisplacement * layer.bgLayerInfo.LayerSpeed / 100;
            //Debug.Log("TEX OFFSET " + layerNewOffset);
            layer.material.SetTextureOffset("_MainTex", layerNewOffset);
        }
    }
}
