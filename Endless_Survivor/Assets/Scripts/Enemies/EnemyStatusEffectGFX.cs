using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusEffectGFX
{
    public GridSpriteInfo statusSprite;
    public Material statusMaterial;

    public EnemyStatusEffectGFX(GridSpriteInfo statusSprite, Material statusMaterial)
    {
        this.statusSprite = statusSprite;
        this.statusMaterial = statusMaterial;
    }
}