using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    static ParticleManager instance;
    public static ParticleManager pm { get { return instance; } }
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }
    public ParticleSystem SpawnParticles(ParticleConfig config)
    {
        var instantiatedParticles = Instantiate(config.particlesPrefab, config.particlesParentTransform);
        if(config.particlesDuration >= 0)
        {
            Destroy(instantiatedParticles.gameObject, config.particlesDuration);
        }
        instantiatedParticles.transform.position = config.particlesPosition;
        instantiatedParticles.transform.rotation = config.particlesRotation;
        return instantiatedParticles;
    }
}
