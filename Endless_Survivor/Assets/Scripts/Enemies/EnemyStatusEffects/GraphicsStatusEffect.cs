using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsStatusEffect : EnemyStatusEffect
{
    new public static bool isUsable => true;
    [SerializeField] Sprite _statusEffectIndicator;
    [SerializeField] Material _statusEffectMaterial;
    public override void Initialize(EnemyControl affectedEnemyControl, EnemyStatusEffect original)
    {
        base.Initialize(affectedEnemyControl, original);
        var graphicsOriginal = (GraphicsStatusEffect)original;
        _statusEffectIndicator = graphicsOriginal._statusEffectIndicator;
        _statusEffectMaterial = graphicsOriginal._statusEffectMaterial;
    }
    public override void Start()
    {
        base.Start();
        AffectedEnemyControl.StatusEffectManager.AddStatusGraphics(_statusEffectIndicator, _statusEffectMaterial, this);
    }
    public override void End()
    {
        base.End();
        AffectedEnemyControl.StatusEffectManager.RemoveStatusGraphics(this);
    }
}
