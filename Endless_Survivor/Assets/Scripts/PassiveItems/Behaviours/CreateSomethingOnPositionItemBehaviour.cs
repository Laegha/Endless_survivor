using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CreateSomethingOnPositionItemBehaviour : PassiveItemBehaviour
{
    new public static bool isUsable => false;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        
    }
    public void CreateSomething(Vector2 position)
    {

    }
}
