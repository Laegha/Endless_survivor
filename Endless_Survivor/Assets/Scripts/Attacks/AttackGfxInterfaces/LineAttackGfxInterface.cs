using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LineAttackGfxInterface : AttackGfxInterface
{
    public Material rayMaterial;
    public float rayWidth;

    public void ApplyGfx(LineRenderer lineRenderer)
    {
        lineRenderer.material = rayMaterial;
        lineRenderer.startWidth = rayWidth;
        lineRenderer.endWidth = rayWidth;
    }
}
