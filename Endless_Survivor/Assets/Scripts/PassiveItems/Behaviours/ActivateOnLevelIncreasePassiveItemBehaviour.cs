using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnLevelIncreasePassiveItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] RandomBetweenTwoConstants _levelsToActivate;
    [SerializeField] string[] _activatedBehaviours;

    int _currGoal = 0;
    int _levelsIncreased = 0;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var activateAfterLevelsOriginal = original as ActivateOnLevelIncreasePassiveItemBehaviour;
        _levelsToActivate = activateAfterLevelsOriginal._levelsToActivate;
        _activatedBehaviours = activateAfterLevelsOriginal._activatedBehaviours;
        _currGoal = (int)_levelsToActivate.rand;
        behaviourManager.onIntensityIncrease += IncreaseCounter;
    }
    void IncreaseCounter()
    {
        _levelsIncreased++;
        if (_levelsIncreased < _currGoal)
            return;

        _currGoal = (int)_levelsToActivate.rand;
        _levelsIncreased = 0;
        foreach (string id in _activatedBehaviours)
        {
            var activatedBehaviour = BehaviourManager.ItemBehaviours.Find(x => x.BehaviourId == id);
            activatedBehaviour.Activate();
        }

    }
    public override void RemoveBehaviour()
    {

    }

}
