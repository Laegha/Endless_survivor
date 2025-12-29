using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectGFX
{
    public GridSpriteInfo statusSprite;
    public MaterialOverride statusMaterial;
    public ParticleSystem statusParticles;

    public StatusEffectGFX(GridSpriteInfo statusSprite, MaterialOverride statusMaterial, ParticleSystem statusParticles)
    {
        this.statusSprite = statusSprite;
        this.statusMaterial = statusMaterial;
        this.statusParticles = statusParticles;
    }
}