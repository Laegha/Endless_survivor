using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RemoveAfterHPLostPassiveItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => 1;

    [SerializeField] int _hpLostToRemove;
    int _damageCounter = 0;

    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var removeAfterHpOriginal = original as RemoveAfterHPLostPassiveItemBehaviour;
        _hpLostToRemove = removeAfterHpOriginal._hpLostToRemove;
        behaviourManager.onPlayerDamaged += IncreaseDamageCounter;
    }

    void IncreaseDamageCounter(int recievedDamage)
    {
        _damageCounter += recievedDamage;
        if(_damageCounter >= _hpLostToRemove)
        {
            PlayerControl.pc.PassiveItemManager.RemovePassiveItem(BehaviourManager.PassiveItem);
        }
    }

    public override void RemoveBehaviour()
    {

    }
}
