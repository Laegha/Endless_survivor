using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActivateOnPlayerHitItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] string[] _activatedBehaviours;
    [Range(0,100)][SerializeField] float _activationChance;

    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var activateOnHitOriginal = original as ActivateOnPlayerHitItemBehaviour;
        _activatedBehaviours = activateOnHitOriginal._activatedBehaviours;
        _activationChance = activateOnHitOriginal._activationChance;

        behaviourManager.onPlayerDamaged += ActivateBehaviours;
    }

    void ActivateBehaviours(int _)
    {
        Debug.Log("ACTIVATING ON PLAYR HIT");
        if (Random.Range(0, 100) > _activationChance)
            return;

        foreach (var behaviourId in _activatedBehaviours)
        {
            BehaviourManager.ItemBehaviours.Find(behaviour => behaviour.BehaviourId == behaviourId).Activate();
        }
    }
    public override void RemoveBehaviour()
    {

    }
}
