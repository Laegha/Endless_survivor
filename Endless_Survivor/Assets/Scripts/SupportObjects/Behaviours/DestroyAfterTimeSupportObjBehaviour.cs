using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTimeSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] RandomBetweenTwoConstants _destroyTime;
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var destroyAfterTimeOriginal = original as DestroyAfterTimeSupportObjBehaviour;
        _destroyTime = destroyAfterTimeOriginal._destroyTime;

        DestroyObj(_destroyTime.rand);
    }
}
