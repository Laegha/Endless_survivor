using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnDestroySupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] DamageInfo _explosionDamage;
    [SerializeField] float _explosionRadius;
    [SerializeField] ParticleSystem _explosionParticles;
    [SerializeField] float _particlesDuration;
    [SerializeField] LayerMask _affectedLayers;

    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var explosionOriginal = original as ExplodeOnDestroySupportObjBehaviour;
        _explosionDamage = explosionOriginal._explosionDamage;
        _explosionRadius = explosionOriginal._explosionRadius;
        _explosionParticles = explosionOriginal._explosionParticles;
        _particlesDuration = explosionOriginal._particlesDuration;
        _affectedLayers = explosionOriginal._affectedLayers;

        OnDestroyed += Explode;
    }
    public void Explode()
    {
        ParticleConfig explosionParticleConfig = new(_explosionParticles, ObjControl.transform.position, Quaternion.identity, _particlesDuration);
        ParticleManager.pm.SpawnParticles(explosionParticleConfig);
        var affectedObjs = Physics2D.OverlapCircleAll(ObjControl.transform.position, _explosionRadius, _affectedLayers);
        foreach (var obj in affectedObjs)
        {
            var objHP = obj.GetComponent<HP>();
            if (objHP == null)
                continue;
            objHP.TakeDamage((int)_explosionDamage.CalculatedDamage);
        }
    }
}
