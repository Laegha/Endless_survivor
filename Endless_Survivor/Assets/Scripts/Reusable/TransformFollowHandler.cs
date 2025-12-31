using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFollowHandler
{
    public Transform child;
    public Transform parent;
    public bool copyPosition;
    public bool copyRotation;
    public Vector2 childLocalPos;

    public TransformFollowHandler(Transform child, Transform parent, bool copyPosition, bool copyRotation, Vector2 childLocalPos = default)
    {
        this.child = child;
        this.parent = parent;
        this.copyPosition = copyPosition;
        this.copyRotation = copyRotation;
        this.childLocalPos = childLocalPos;
    }

    public void Update()
    {
        if (child == null || parent == null)
            return;
        if(copyPosition)
            child.position = (Vector2)parent.position + childLocalPos;
        if(copyRotation)
            child.rotation = parent.rotation;
    }
}
