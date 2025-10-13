using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusEffectGFX
{
    public GridSpriteInfo statusSprite;
    public MaterialOverride statusMaterial;
    public ParticleSystem statusParticles;

    public EnemyStatusEffectGFX(GridSpriteInfo statusSprite, MaterialOverride statusMaterial, ParticleSystem statusParticles)
    {
        this.statusSprite = statusSprite;
        this.statusMaterial = statusMaterial;
        this.statusParticles = statusParticles;
    }
}