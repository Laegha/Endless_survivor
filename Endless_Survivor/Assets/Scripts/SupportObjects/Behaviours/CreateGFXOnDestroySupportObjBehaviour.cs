using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGFXOnDestroySupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] CustomAnimation _createdAnimation;
    [SerializeField] float _animationDuration;
    [SerializeField] ParticleSystem _createdParticles;
    [SerializeField] float _particlesDuration;
    [SerializeField] Vector2 _creationOffset;
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var createGfxOriginal = original as CreateGFXOnDestroySupportObjBehaviour;
        _createdAnimation = createGfxOriginal._createdAnimation;
        _animationDuration = createGfxOriginal._animationDuration;
        _createdParticles = createGfxOriginal._createdParticles;
        _particlesDuration = createGfxOriginal._particlesDuration;
        _creationOffset = createGfxOriginal._creationOffset;

        OnDestroyed += CreateGFX;
    }

    void CreateGFX()
    {
        Vector2 createPosition = Utility.IsTileUsable((Vector2)ObjControl.transform.position + _creationOffset) ? (Vector2)ObjControl.transform.position + _creationOffset : ObjControl.transform.position;
        if (_createdAnimation != null)
            AnimatedObjsManager.aom.SpawnAnimatedObj(new(_createdAnimation, createPosition, Quaternion.identity, _animationDuration, null, false, false));
        if (_createdParticles != null)
            ParticleManager.pm.SpawnParticles(new(_createdParticles, createPosition, Quaternion.identity, _particlesDuration));
    }       
}
