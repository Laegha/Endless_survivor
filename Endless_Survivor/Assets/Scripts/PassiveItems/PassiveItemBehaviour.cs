using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PassiveItemBehaviour
{
    public static bool isUsable => false;
    public virtual void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {

    }
}
