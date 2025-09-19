using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleConfig
{
    public ParticleSystem particlesPrefab;
    public float particlesDuration;//pass a negative number on the constructor for no duration limit
    public Transform particlesParentTransform;
    public bool copyPosition;
    public bool copyRotation;
    public Vector3 particlesPosition;
    public Quaternion particlesRotation;

    public ParticleConfig(ParticleSystem particlesPrefab, Vector3 particlesPosition, Quaternion particlesRotation, float particlesDuration = 1f, Transform particlesParentTransform = null, bool copyPosition = true, bool copyRotation = true)
    {
        this.particlesPrefab = particlesPrefab;
        this.particlesPosition = particlesPosition;
        this.particlesRotation = particlesRotation;
        this.particlesDuration = particlesDuration;
        this.particlesParentTransform = particlesParentTransform;
        this.copyPosition = copyPosition;
        this.copyRotation = copyRotation;
    }
}
