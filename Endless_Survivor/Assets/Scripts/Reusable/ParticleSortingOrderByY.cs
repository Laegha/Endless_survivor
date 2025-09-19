using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSortingOrderByY : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;
    ParticleSystemRenderer _particleRenderer;

    private void Start()
    {
        _particleRenderer = _particleSystem.GetComponent<ParticleSystemRenderer>();
    }
    private void LateUpdate()
    {
        _particleRenderer.sortingOrder = (int)(-transform.position.y * 100);
    }
}
