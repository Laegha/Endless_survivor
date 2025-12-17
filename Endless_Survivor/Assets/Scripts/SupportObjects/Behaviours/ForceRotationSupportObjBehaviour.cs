using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceRotationSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] float _rotation;

    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var forceRotationOriginal = original as ForceRotationSupportObjBehaviour;
        _rotation = forceRotationOriginal._rotation;

        OnLateUpdate += ForceRotation;
    }

    void ForceRotation()
    {
        var children = ObjControl.GetComponentsInChildren<Transform>();
        foreach(var child in children) 
            child.rotation = Quaternion.Euler(child.rotation.x, child.rotation.y, _rotation);

    }
}
