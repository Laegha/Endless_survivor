using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveOnActivateItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => 1;
    public override void Activate()
    {
        base.Activate();
        PlayerControl.pc.PassiveItemManager.RemovePassiveItem(BehaviourManager.PassiveItem);
    }
    public override void RemoveBehaviour()
    {

    }
}
