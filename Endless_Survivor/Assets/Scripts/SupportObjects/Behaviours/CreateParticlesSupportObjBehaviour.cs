using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateParticlesSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] Vector2 _particlesOffset;
    [SerializeField] ParticleSystem _particles;
    [SerializeField] float _particlesDuration;
    [SerializeField] bool _isChildOfSupportObj;

    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var createParticlesOriginal = original as CreateParticlesSupportObjBehaviour;
        _particlesOffset = createParticlesOriginal._particlesOffset;
        _particles = createParticlesOriginal._particles;
        _particlesDuration = createParticlesOriginal._particlesDuration;
        _isChildOfSupportObj = createParticlesOriginal._isChildOfSupportObj;

        OnStart += CreateParticles;
    }

    void CreateParticles()
    {
        ParticleManager.pm.SpawnParticles(new(_particles, _particlesOffset + (_isChildOfSupportObj ? Vector2.zero : ObjControl.transform.position), Quaternion.identity, _particlesDuration, (_isChildOfSupportObj ? ObjControl.transform : null)));
    }
}
