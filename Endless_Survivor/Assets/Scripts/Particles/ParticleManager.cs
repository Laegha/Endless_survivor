using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    static ParticleManager instance;
    public static ParticleManager pm { get { return instance; } }
    List<TransformFollowHandler> _particlesFollowing = new List<TransformFollowHandler>();
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }
    public ParticleSystem SpawnParticles(ParticleConfig config)
    {
        if (config.particlesPrefab == null)
            return null;
        var instantiatedParticles = Instantiate(config.particlesPrefab);
        if (config.particlesParentTransform != null)
            _particlesFollowing.Add(new(instantiatedParticles.transform, config.particlesParentTransform, config.copyPosition, config.copyRotation, config.particlesPosition));
        if(config.particlesDuration >= 0)
        {
            Destroy(instantiatedParticles.gameObject, config.particlesDuration);
        }
        instantiatedParticles.transform.position = config.particlesPosition;
        instantiatedParticles.transform.rotation = config.particlesRotation;
        return instantiatedParticles;
    }
    private void Update()
    {
        var particlesFollowingCopy = new List<TransformFollowHandler>(_particlesFollowing);
        foreach(var particleFollowing in particlesFollowingCopy)
        {
            if(particleFollowing.parent == null)
                DestroyImmediate(particleFollowing.child.gameObject);
            if(particleFollowing.child == null)
            {
                _particlesFollowing.Remove(particleFollowing);
                continue;
            }
            particleFollowing.Update();
        }
    }
}
