using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class InflictPlayerStatusInAreaEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] float _areaRadius;
    [SerializeField] Vector2 _areaOffset;
    [SerializeField] PlayerStatusEffectData[] _inflictedEffects;
    [SerializeField] float _inflictionCooldown;
    [SerializeField] CustomAnimation _inflictionAreaAnimation;

    float _cooldownTimer;
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var inflictStatusOriginal = original as InflictPlayerStatusInAreaEnemyBehaviour;
        _areaRadius = inflictStatusOriginal._areaRadius;
        _areaOffset = inflictStatusOriginal._areaOffset;
        _inflictedEffects = inflictStatusOriginal._inflictedEffects;
        _inflictionCooldown = inflictStatusOriginal._inflictionCooldown;
        _cooldownTimer = _inflictionCooldown;
        _inflictionAreaAnimation = inflictStatusOriginal._inflictionAreaAnimation;
    }
    public override void Start()
    {
        base.Start();
        if(_inflictionAreaAnimation != null && _inflictionAreaAnimation.Frames.Length > 0)
        {
            AnimatedObjConfig areaObjConfig = new(_inflictionAreaAnimation, _areaOffset, Quaternion.identity, -1, EnemyControl.transform);
            var areaObj = AnimatedObjsManager.aom.SpawnAnimatedObj(areaObjConfig);
            areaObj.transform.SetParent(EnemyControl.transform);
        }
    }
    public override void PassiveUpdate()
    {
        base.PassiveUpdate();
        _cooldownTimer -= Time.deltaTime;
        if (_cooldownTimer > 0)
            return;
        var objsInArea = Physics2D.OverlapCircleAll((Vector2)EnemyControl.transform.position + _areaOffset, _areaRadius, Utility.GetCollidableLayers("PlayerDetector"));
        if (objsInArea.Length == 0)
            return;
        _cooldownTimer = _inflictionCooldown;
        foreach (var effect in _inflictedEffects)
        {
            PlayerControl.pc.StatusEffectManager.AddEffects(effect.StatusEffects, effect);

        }
    }
}
