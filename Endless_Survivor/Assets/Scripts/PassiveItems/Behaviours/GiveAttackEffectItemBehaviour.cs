using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GiveAttackEffectItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] AttackEffectData[] _givenAttackEffects ;

    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var giveAttackEffectOriginal = original as GiveAttackEffectItemBehaviour;
        _givenAttackEffects = giveAttackEffectOriginal._givenAttackEffects;
        behaviourManager.onPicked += AddEffect;
    }
    void AddEffect()
    {
        foreach (var effect in _givenAttackEffects)
        {
            PlayerControl.pc.EffectsHolder.AddEffect(effect);
        }
    }
}
