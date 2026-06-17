using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealPlayerOnActivateItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] int _healAmmount;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var healPlayerOriginal = original as HealPlayerOnActivateItemBehaviour;
        _healAmmount = healPlayerOriginal._healAmmount;
    }
    public override void Activate()
    {
        base.Activate();
        PlayerControl.pc.PlayerHPManager.Heal(_healAmmount);
    }
    public override void RemoveBehaviour()
    {

    }
}
