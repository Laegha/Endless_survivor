using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBehavioursOverTimeEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] RandomBetweenTwoConstants _timeBetweenTriggering;
    [SerializeField] string[] _triggeredBehaviours;
    float _timer;
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var triggerOverTimeOriginal = original as TriggerBehavioursOverTimeEnemyBehaviour;
        _timeBetweenTriggering = triggerOverTimeOriginal._timeBetweenTriggering;
        _triggeredBehaviours = triggerOverTimeOriginal._triggeredBehaviours;
        _timer = _timeBetweenTriggering.rand;
    }
    public override void PassiveUpdate()
    {
        base.PassiveUpdate();
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _timer = _timeBetweenTriggering.rand;
            //Activate behaviours
            foreach (var behaviour in _triggeredBehaviours)
                EnemyControl.BehaviourManager.ActivateBehaviour(behaviour);
            EnemyControl.BehaviourManager.ActivateBehaviour(this);
        }
    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        foreach (var behaviour in _triggeredBehaviours)
        {
            if (EnemyControl.BehaviourManager.GetBehaviour(behaviour).IsActive)
                return;
        }
        KillBehaviour();
    }

}
