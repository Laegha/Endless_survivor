using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BulletData
{
    [SerializeField] Sprite _bulletSprite;
    [SerializeField] Vector2 _colliderSize;
    public Sprite BulletSprite {  get { return _bulletSprite; } }
    public Vector2 ColliderSize { get { return _colliderSize; } }
}
