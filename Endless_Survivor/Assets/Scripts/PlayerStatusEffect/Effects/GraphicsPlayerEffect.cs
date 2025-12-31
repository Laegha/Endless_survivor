using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsPlayerStatusEffect : PlayerStatusEffect
{
    new public static int maxStacks => 1;
    [SerializeField] Sprite _statusEffectIndicator;
    [SerializeField] Material _statusEffectMaterial;
    [SerializeField] ParticleSystem _statusEffectParticles;
    [SerializeField] Vector2 _particlesOffset;
    public override void Initialize(PlayerStatusEffect original)
    {
        base.Initialize(original);
        var graphicsOriginal = (GraphicsPlayerStatusEffect)original;
        _statusEffectIndicator = graphicsOriginal._statusEffectIndicator;
        _statusEffectMaterial = graphicsOriginal._statusEffectMaterial;
        _statusEffectParticles = graphicsOriginal._statusEffectParticles;
        _particlesOffset = graphicsOriginal._particlesOffset;
    }
    public override void Start()
    {
        base.Start();
        PlayerControl.pc.StatusEffectManager.AddStatusGraphics(_statusEffectIndicator, _statusEffectMaterial, _statusEffectParticles, _particlesOffset, this);
    }
    public override void End()
    {
        base.End();
        PlayerControl.pc.StatusEffectManager.RemoveStatusGraphics(this);
    }
}
