using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBehavioursOverTimeOnActiveEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] RandomBetweenTwoConstants _timeBetweenTriggering;
    [SerializeField] string[] _triggeredBehaviours;
    float _timer;
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var triggerOverTimeOriginal = original as TriggerBehavioursOverTimeOnActiveEnemyBehaviour;
        _timeBetweenTriggering = triggerOverTimeOriginal._timeBetweenTriggering;
        _triggeredBehaviours = triggerOverTimeOriginal._triggeredBehaviours;
        _timer = _timeBetweenTriggering.rand;
    }
    public override void PassiveUpdate()
    {
        base.PassiveUpdate();
        if(_timer > 0)
        {  
            _timer -= Time.deltaTime;
        }
    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        if (_timer <= 0)
        {
            _timer = _timeBetweenTriggering.rand;
            //Activate behaviours+
            TriggerBehaviours();
        }
    }

    void TriggerBehaviours()
    {
        foreach (var behaviour in _triggeredBehaviours)
            EnemyControl.BehaviourManager.ActivateBehaviour(behaviour);
    }

}
