using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ThrowAtEnemySupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 1;

    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var throwOriginal = original as ThrowAtEnemySupportObjBehaviour;
    }
}
