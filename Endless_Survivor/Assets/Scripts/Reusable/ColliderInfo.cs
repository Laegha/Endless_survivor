using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ColliderInfo
{
    public enum ColliderType
    {
        Box,
        Capsule,
        Circle
    }

    [SerializeField] Vector2 _colliderLocalPos;
    [SerializeField] ColliderType _colliderType;
    [SerializeField] Vector2 _colliderSize;
    [SerializeField] CapsuleDirection2D _capsuleDirection;
    [SerializeField] float _colliderRadius;
    [SerializeField] SingleUnityLayer _collidingLayer;
    [SerializeField] bool _isTrigger;


    public ColliderInfo(ColliderType colliderType, Vector2 colliderSize, float colliderRadius, Vector2 colliderLocalPos, CapsuleDirection2D capsuleDirection, SingleUnityLayer collidingLayer, bool isTrigger)
    {
        _colliderType = colliderType;
        _colliderSize = colliderSize;
        _colliderRadius = colliderRadius;
        _colliderLocalPos = colliderLocalPos;
        _capsuleDirection = capsuleDirection;
        _collidingLayer = collidingLayer;
        _isTrigger = isTrigger;
    }
    public ColliderInfo(ColliderInfo original)
    {
        _colliderType = original._colliderType;
        _colliderSize = original._colliderSize;
        _colliderRadius = original._colliderRadius;
        _colliderLocalPos = original._colliderLocalPos;
        _capsuleDirection = original._capsuleDirection;
        _collidingLayer = original._collidingLayer;
        _isTrigger = original._isTrigger;

    }

    public GameObject GenerateColliderObj(Transform colliderParent)
    {
        GameObject colObj = new GameObject(_colliderType.ToString() + " Collider");
        colObj.transform.SetParent(colliderParent);
        colObj.transform.localPosition = _colliderLocalPos;
        Collider2D collider = null;
        switch (_colliderType)
        {
            case ColliderType.Box:
                var boxCollider = colObj.AddComponent<BoxCollider2D>();
                boxCollider.size = _colliderSize;
                collider = boxCollider;
                break;
            case ColliderType.Capsule:
                var capsuleCollider = colObj.AddComponent<CapsuleCollider2D>();
                capsuleCollider.size = _colliderSize;
                capsuleCollider.direction = _capsuleDirection;
                collider = capsuleCollider;
                break;
            case ColliderType.Circle:
                var circleCollider = colObj.AddComponent<CircleCollider2D>();
                circleCollider.radius = _colliderRadius;
                collider = circleCollider;
                break;
        }
        collider.isTrigger = _isTrigger;
        collider.gameObject.layer = _collidingLayer.Mask;
        return colObj;
    }
}
