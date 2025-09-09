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
    [SerializeField] CustomAnimation _attackVfxAnimation;
    [SerializeField] int _damageFrame;
    [SerializeField] ParticleSystem _attackParticles;
    [SerializeField] float _particleDuration = -1;

    public bool IsCircle { get { return _isCircle; } }
    public float CircleRadius { get { return _attackCircleRadius; } }
    public Vector2 BoxSize {  get { return _attackBoxSize; } }
    public CustomAnimation AttackVfxAnimation { get { return _attackVfxAnimation; } }
    public int DamageFrame {  get { return _damageFrame; } }
    public ParticleSystem AttackParticles { get { return _attackParticles; } }
    public float ParticleDuration { get { return _particleDuration; } }

    public MeleeData(MeleeData original)
    {
        _isCircle = original._isCircle;
        _attackCircleRadius = original._attackCircleRadius;
        _attackBoxSize = original._attackBoxSize;
        _attackVfxAnimation = original._attackVfxAnimation;
        _damageFrame = original._damageFrame;
        _attackParticles = original._attackParticles;
        _particleDuration = original._particleDuration;

    }
}
