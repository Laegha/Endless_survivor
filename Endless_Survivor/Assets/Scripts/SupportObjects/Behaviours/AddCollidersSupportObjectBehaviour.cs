using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AddCollidersSupportObjectBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] List<DamagingColliderInfo> _addingColliders = new();
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var addCollidersOriginal = original as AddCollidersSupportObjectBehaviour;
        _addingColliders = addCollidersOriginal._addingColliders;
        OnStart += CreateColliders;
    }

    void CreateColliders()
    {
        Transform collidersHolder = new GameObject("AddedColliders").transform;
        collidersHolder.SetParent(ObjControl.transform);
        collidersHolder.localPosition = Vector3.zero;
        foreach (var colliderInfo in _addingColliders)
        {
            colliderInfo.GenerateDamagingColliderObj(collidersHolder);
        }
    }

}
