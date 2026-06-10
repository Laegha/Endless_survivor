using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySupportObjOnContactSupportObjBehaviour : UseAreaAroundSupportObjBehaviour
{
    new public static int maxStacks => 1;

    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        OnObjEnterArea += (GameObject collided) => DestroyObj();

    }
}
