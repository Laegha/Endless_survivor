using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnStartItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] string[] _activatingIds;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var activateOnStartOriginal = original as ActivateOnStartItemBehaviour;
        _activatingIds = activateOnStartOriginal._activatingIds;
        behaviourManager.onPicked += ActivateBehaviours;
    }
    void ActivateBehaviours()
    {
        foreach(var id in _activatingIds )
        {
            BehaviourManager.ItemBehaviours.Find(x => x.BehaviourId == id)?.Activate();
        }
    }
    public override void RemoveBehaviour()
    {

    }
}
