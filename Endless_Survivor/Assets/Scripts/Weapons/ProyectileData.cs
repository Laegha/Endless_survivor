using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProyectileData
{
    [SerializeField] Sprite _proyectileSprite;
    [SerializeField] Vector2 _colliderSize;
    [SerializeField] Material _proyectileMaterial;
    [SerializeField] GameObject _particlesPrefab;
    [SerializeField] SFXInfo _shootSFX;
    public Sprite ProyectileSprite {  get { return _proyectileSprite; } }
    public Vector2 ColliderSize { get { return _colliderSize; } }
    public Material ProyectileMaterial { get {  return _proyectileMaterial; } }
    public GameObject ParticlesPrefab {  get { return _particlesPrefab; } }
    public SFXInfo ShootSFX { get { return _shootSFX; } }

    public ProyectileData(ProyectileData original)
    {
        _proyectileSprite = original.ProyectileSprite;
        _colliderSize = original.ColliderSize;
    }
}
