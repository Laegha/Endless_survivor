using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsStatusEffect : EnemyStatusEffect
{
    new public static bool isUsable => true;
    [SerializeField] Sprite _statusEffectIndicator;
    [SerializeField] Material _statusEffectMaterial;
    [SerializeField] ParticleSystem _statusEffectParticles;
    public override void Initialize(EnemyControl affectedEnemyControl, EnemyStatusEffect original, ConditionHolder endCondition)
    {
        base.Initialize(affectedEnemyControl, original, endCondition);
        var graphicsOriginal = (GraphicsStatusEffect)original;
        _statusEffectIndicator = graphicsOriginal._statusEffectIndicator;
        _statusEffectMaterial = graphicsOriginal._statusEffectMaterial;
        _statusEffectParticles = graphicsOriginal._statusEffectParticles;
    }
    public override void Start()
    {
        base.Start();
        AffectedEnemyControl.StatusEffectManager.AddStatusGraphics(_statusEffectIndicator, _statusEffectMaterial, _statusEffectParticles, this);
    }
    public override void End()
    {
        base.End();
        AffectedEnemyControl.StatusEffectManager.RemoveStatusGraphics(this);
    }
}
