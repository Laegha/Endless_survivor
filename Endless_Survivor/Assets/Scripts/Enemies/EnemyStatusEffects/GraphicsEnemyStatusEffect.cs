using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsEnemyStatusEffect : EnemyStatusEffect
{
    new public static bool isUsable => true;
    [SerializeField] Sprite _statusEffectIndicator;
    [SerializeField] Material _statusEffectMaterial;
    [SerializeField] ParticleSystem _statusEffectParticles;
    [SerializeField] Vector2 _particlesOffset;
    public override void Initialize(EnemyControl affectedEnemyControl, EnemyStatusEffect original)
    {
        base.Initialize(affectedEnemyControl, original);
        var graphicsOriginal = (GraphicsEnemyStatusEffect)original;
        _statusEffectIndicator = graphicsOriginal._statusEffectIndicator;
        _statusEffectMaterial = graphicsOriginal._statusEffectMaterial;
        _statusEffectParticles = graphicsOriginal._statusEffectParticles;
        _particlesOffset = graphicsOriginal._particlesOffset;
    }
    public override void Start()
    {
        base.Start();
        AffectedEnemyControl.StatusEffectManager.AddStatusGraphics(_statusEffectIndicator, _statusEffectMaterial, _statusEffectParticles, _particlesOffset, this);
    }
    public override void End()
    {
        base.End();
        AffectedEnemyControl.StatusEffectManager.RemoveStatusGraphics(this);
    }
}
