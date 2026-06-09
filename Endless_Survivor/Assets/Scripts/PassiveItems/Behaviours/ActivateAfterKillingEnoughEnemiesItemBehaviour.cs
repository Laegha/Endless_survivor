using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAfterKillingEnoughEnemiesItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] string[] _activatedBehavioursIds;
    [SerializeField] int _killingAmount;
    [SerializeField] bool _loop;
    [SerializeField] float _waitTimeToResetCount;

    int _killCount;
    float _timerToResetCount;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var activateAfterKillsOriginal = original as ActivateAfterKillingEnoughEnemiesItemBehaviour;
        _activatedBehavioursIds = activateAfterKillsOriginal._activatedBehavioursIds;
        _killingAmount = activateAfterKillsOriginal._killingAmount;
        _loop = activateAfterKillsOriginal._loop;
        _waitTimeToResetCount = activateAfterKillsOriginal._waitTimeToResetCount;
        behaviourManager.onEnemyKilled += IncreaseCount;
        behaviourManager.onUpdate += ReduceTimer;
    }
    void ReduceTimer()
    {
        if(_timerToResetCount > 0)
        {
            _timerToResetCount-= Time.deltaTime;
        }
    }
    void IncreaseCount(EnemyControl placeholder)
    {
        if ((!_loop && _killCount >= _killingAmount ) || _timerToResetCount > 0)
            return;

        _killCount++;

        if (_killCount < _killingAmount)
            return;

        ActivateBehaviours();
        
        if (!_loop)
            return;

        _timerToResetCount = _waitTimeToResetCount;
        _killCount = 0;
    }
    void ActivateBehaviours()
    {
        foreach(var id in _activatedBehavioursIds)
        {
            var activatingBehaviour = BehaviourManager.ItemBehaviours.Find(behaviour => behaviour.BehaviourId == id);
            activatingBehaviour.Activate();
        }
    }
    public override void RemoveBehaviour()
    {

    }
}
