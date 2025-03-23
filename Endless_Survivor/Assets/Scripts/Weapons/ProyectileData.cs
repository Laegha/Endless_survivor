using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProyectileData
{
    [SerializeField] Sprite _proyectileSprite;
    [SerializeField] Vector2 _colliderSize;
    public Sprite ProyectileSprite {  get { return _proyectileSprite; } }
    public Vector2 ColliderSize { get { return _colliderSize; } }
    
    public ProyectileData(ProyectileData original)
    {
        _proyectileSprite = original.ProyectileSprite;
        _colliderSize = original.ColliderSize;
    }
}
