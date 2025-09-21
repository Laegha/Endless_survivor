using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFollowHandler
{
    public Transform child;
    public Transform parent;
    public bool copyPosition;
    public bool copyRotation;

    public TransformFollowHandler(Transform child, Transform parent, bool copyPosition, bool copyRotation)
    {
        this.child = child;
        this.parent = parent;
        this.copyPosition = copyPosition;
        this.copyRotation = copyRotation;
    }

    public void Update()
    {
        if (child == null || parent == null)
            return;
        if(copyPosition)
            child.position = parent.position;
        if(copyRotation)
            child.rotation = parent.rotation;
    }
}
