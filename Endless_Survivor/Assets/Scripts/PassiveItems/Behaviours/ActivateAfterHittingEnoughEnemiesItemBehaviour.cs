using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAfterHittingEnoughEnemiesItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] RandomBetweenTwoConstants _hitsNeeded;
    [SerializeField] string[] _activatedBehaviours;

    int _currGoal = 0;
    int _hitEnemies = 0;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var activateAfterHttingOriginal = original as ActivateAfterHittingEnoughEnemiesItemBehaviour;
        _hitsNeeded = activateAfterHttingOriginal._hitsNeeded;
        _activatedBehaviours = activateAfterHttingOriginal._activatedBehaviours;
        _currGoal = (int)_hitsNeeded.rand;
        behaviourManager.onEnemyHit += IncreaseCounter;
    }
    void IncreaseCounter(EnemyControl hitEnemy, Attack usedAttack)
    {
        _hitEnemies++;
        Debug.Log("CURR HIT ENEMIES " + _hitEnemies);
        if (_hitEnemies < _currGoal)
            return;

        _currGoal = (int)_hitsNeeded.rand;
        _hitEnemies = 0;
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
