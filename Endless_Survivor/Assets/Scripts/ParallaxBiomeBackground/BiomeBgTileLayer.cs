using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeBgTileLayer
{
    public Material material;
    public SpriteRenderer renderer;
    public BiomeBgLayerInfo bgLayerInfo;

    public BiomeBgTileLayer(Material material, SpriteRenderer renderer, BiomeBgLayerInfo bgLayerInfo)
    {
        this.material = material;
        this.renderer = renderer;
        this.bgLayerInfo = bgLayerInfo;
    }
}
