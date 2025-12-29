using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePlayerStatusEffectOnActivateItemBehaviour : PassiveItemBehaviour
{
    [SerializeField] PlayerStatusEffectData _givenEffectData;
    new public static int maxStacks => -1;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var giveStatusOriginal = original as GivePlayerStatusEffectOnActivateItemBehaviour;
        _givenEffectData = giveStatusOriginal._givenEffectData;
    }
    public override void Activate()
    {
        base.Activate();
        PlayerControl.pc.StatusEffectManager.AddEffects(_givenEffectData.StatusEffects, _givenEffectData);

    }
}
