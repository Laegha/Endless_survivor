using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusEffectGFX
{
    public GridSpriteInfo statusSprite;
    public MaterialOverride statusMaterial;

    public EnemyStatusEffectGFX(GridSpriteInfo statusSprite, MaterialOverride statusMaterial)
    {
        this.statusSprite = statusSprite;
        this.statusMaterial = statusMaterial;
    }
}