using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class MeleeData
{
    [SerializeField] bool _isCircle;
    [Header("Use if IsCircle")]
    [SerializeField] float _attackCircleRadius;
    [Header("Use if not IsCircle")]
    [SerializeField] Vector2 _attackBoxSize;
    [Header("")]
    [SerializeField] Vector2 _attackOffset;
    [SerializeField] CustomAnimation _attackVfxAnimation;
    [SerializeField] bool _dropVfxOnDamage;
    [SerializeField] int _vfxRendererOffset;
    [SerializeField] ParticleSystem _attackParticles;
    [SerializeField] float _particleDuration = -1;

    public bool IsCircle { get { return _isCircle; } }
    public float CircleRadius { get { return _attackCircleRadius; } }
    public Vector2 BoxSize {  get { return _attackBoxSize; } }
    public Vector2 AttackOffset {  get { return _attackOffset; } }
    public CustomAnimation AttackVfxAnimation { get { return _attackVfxAnimation; } }
    public bool DropVfxOnAttack { get { return _dropVfxOnDamage; } }
    public int VfxRendererOffset { get { return _vfxRendererOffset; } }
    public ParticleSystem AttackParticles { get { return _attackParticles; } }
    public float ParticleDuration { get { return _particleDuration; } }

    public MeleeData(MeleeData original)
    {
        _isCircle = original._isCircle;
        _attackCircleRadius = original._attackCircleRadius;
        _attackBoxSize = original._attackBoxSize;
        _attackOffset = original._attackOffset;
        _attackVfxAnimation = original._attackVfxAnimation;
        _dropVfxOnDamage = original._dropVfxOnDamage;
        _vfxRendererOffset = original._vfxRendererOffset;
        _attackParticles = original._attackParticles;
        _particleDuration = original._particleDuration;

    }
}
