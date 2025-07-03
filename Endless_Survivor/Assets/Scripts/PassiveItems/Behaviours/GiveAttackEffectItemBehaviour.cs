using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveAttackEffectItemBehaviour : PassiveItemBehaviour
{
    new public static bool isUsable => true;
    [SerializeField] AttackEffectData[] _givenAttackEffects;

    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        behaviourManager.onPicked += AddEffect;
        var giveAttackEffectOriginal = original as GiveAttackEffectItemBehaviour;
        _givenAttackEffects = giveAttackEffectOriginal._givenAttackEffects;
    }
    void AddEffect()
    {
        foreach (var effect in _givenAttackEffects)
        {
            PlayerControl.pc.EffectsHolder.AddEffect(effect);
        }
    }
}
