using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ApplyEnemyStatusEffectOnAreaOnDestroySupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] EnemyStatusEffectData[] _appliedStatusEffects;
    [SerializeField] float _applicationRadius;
    [SerializeField] ParticleSystem _applicationParticles;
    [SerializeField] float _particlesDuration;

    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var applyEnemyStatusOriginal = original as ApplyEnemyStatusEffectOnAreaOnDestroySupportObjBehaviour;
        _appliedStatusEffects = applyEnemyStatusOriginal._appliedStatusEffects;
        _applicationRadius = applyEnemyStatusOriginal._applicationRadius;
        _applicationParticles = applyEnemyStatusOriginal._applicationParticles;
        _particlesDuration = applyEnemyStatusOriginal._particlesDuration;

        OnDestroyed += ApplyStatusEffects;
        ParticleConfig particleConfig = new(_applicationParticles, ObjControl.transform.position, Quaternion.identity, _particlesDuration, ObjControl.transform, false, false);
        OnDestroyed += () => ParticleManager.pm.SpawnParticles(particleConfig);
    }

    void ApplyStatusEffects()
    {
        var objsInRange = Physics2D.OverlapCircleAll(ObjControl.transform.position, _applicationRadius);
        List<EnemyControl> enemiesInRange = new();
        foreach(var obj in objsInRange)
        {
            EnemyControl objEnemyControl = obj.transform.root.GetComponentInChildren<EnemyControl>();
            if(objEnemyControl != null)
                enemiesInRange.Add(objEnemyControl);
        }
        foreach(var statusEffect in _appliedStatusEffects)
        {
            foreach(var enemy in enemiesInRange)
            {
                statusEffect.ApplyEffects(enemy.StatusEffectManager);
            }
        }
    }

}
