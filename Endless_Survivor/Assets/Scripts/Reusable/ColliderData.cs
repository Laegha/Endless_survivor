using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ColliderData 
{
    public enum ColliderType
    {
        Capusle,
        Box
    }
    public ColliderType colType;
    public Vector2 size;
    public Vector2 position;
    public CapsuleDirection2D capsuleDirection;
}
