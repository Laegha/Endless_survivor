using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSupportObjBehaviour : SupportObjectBehaviour
{    
    new public static int maxStacks => 1;
    [SerializeField] DamageInfo _explosionDamage;
    [SerializeField] float _explosionRadius;
    [SerializeField] ParticleSystem _explosionParticles;
    [SerializeField] float _particlesDuration;
    [SerializeField] CustomAnimation _explosionAnimation;
    [SerializeField] LayerMask _affectedLayers;

    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        ExplosionSupportObjBehaviour explosionOriginal = original as ExplosionSupportObjBehaviour;
        _explosionDamage = explosionOriginal._explosionDamage;
        _explosionRadius = explosionOriginal._explosionRadius;
        _explosionParticles = explosionOriginal._explosionParticles;
        _particlesDuration = explosionOriginal._particlesDuration;
        _explosionAnimation = explosionOriginal._explosionAnimation;
        _affectedLayers = explosionOriginal._affectedLayers;

        if (_explosionAnimation.Frames.Length > 0)
            ObjControl.Animator.AddAnimations(new(){ _explosionAnimation });
        OnStart += Explode;
    }
    public void Explode()
    {
        ParticleConfig explosionParticleConfig = new(_explosionParticles, ObjControl.transform.position, Quaternion.identity, _particlesDuration);
        ParticleManager.pm.SpawnParticles(explosionParticleConfig);
        var affectedObjs = Physics2D.OverlapCircleAll(ObjControl.transform.position, _explosionRadius, _affectedLayers);
        foreach(var obj in affectedObjs)
        {
            var objHP = obj.GetComponent<HP>();
            if (objHP == null)
                continue;
            objHP.TakeDamage((int)_explosionDamage.CalculatedDamage);
        }
        if(_explosionAnimation.Frames.Length > 0)
        {
            ObjControl.Animator.ChangeAnim(_explosionAnimation);
            GameObject.Destroy(ObjControl.gameObject, _explosionAnimation.AnimDuration);
            return;
        }
        GameObject.Destroy(ObjControl.gameObject);
    }
}
