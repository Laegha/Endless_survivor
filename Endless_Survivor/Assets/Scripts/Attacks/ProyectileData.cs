using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProyectileData
{
    [SerializeField] float _proyectileSpeed;
    [SerializeField] float _proyectileSpread;
    [SerializeField] CustomAnimation _proyectileAnim;
    [SerializeField] Vector2 _colliderSize;
    [SerializeField] CapsuleDirection2D _colliderDirection;
    [SerializeField] Material _proyectileMaterial;
    [SerializeField] GameObject _particlesPrefab;
    [SerializeField] SFXInfo _shootSFX;

    public float ProyectileSpeed { get { return _proyectileSpeed; } }
    public float ProyectileSpread { get { return _proyectileSpread; } }
    public CustomAnimation ProyectileAnim {  get { return _proyectileAnim; } }
    public Vector2 ColliderSize { get { return _colliderSize; } }
    public CapsuleDirection2D ColliderDirection {  get { return _colliderDirection; } }
    public Material ProyectileMaterial { get {  return _proyectileMaterial; } }
    public GameObject ParticlesPrefab {  get { return _particlesPrefab; } }
    public SFXInfo ShootSFX { get { return _shootSFX; } }

    public ProyectileData(ProyectileData original)
    {
        _proyectileSpeed = original._proyectileSpeed;
        _proyectileSpread = original._proyectileSpread;
        _proyectileAnim = original._proyectileAnim;
        _colliderSize = original.ColliderSize;
        _proyectileMaterial = original.ProyectileMaterial;
        _particlesPrefab = original.ParticlesPrefab;
        _shootSFX = original.ShootSFX;
        _colliderDirection = original._colliderDirection;
    }
}
