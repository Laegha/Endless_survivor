using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedObjConfig
{
    public CustomAnimation animation;
    public Vector3 animatedObjPosition;
    public Quaternion animatedObjRotation;
    public float animatedObjDuration;//pass a negative number on the constructor for no duration limit
    public Transform animatedObjParentTransform;
    public bool copyPosition;
    public bool copyRotation;

    public AnimatedObjConfig(CustomAnimation animation, Vector3 animatedObjPosition, Quaternion animatedObjRotation, float animatedObjDuration = 1f, Transform animatedObjParentTransform = null, bool copyPosition = true, bool copyRotation = true)
    {
        this.animation = animation;
        this.animatedObjPosition = animatedObjPosition;
        this.animatedObjRotation = animatedObjRotation;
        this.animatedObjDuration = animatedObjDuration;
        this.animatedObjParentTransform = animatedObjParentTransform;
        this.copyPosition = copyPosition;
        this.copyRotation = copyRotation;
    }
}
