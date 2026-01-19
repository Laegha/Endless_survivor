using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBehavioursOverTimeOnActiveEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] RandomBetweenTwoConstants _timeBetweenTriggering;
    [SerializeField] string[] _triggeredBehaviours;
    float _timer;
    bool _isActivated;
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var triggerOverTimeOriginal = original as TriggerBehavioursOverTimeOnActiveEnemyBehaviour;
        _timeBetweenTriggering = triggerOverTimeOriginal._timeBetweenTriggering;
        _triggeredBehaviours = triggerOverTimeOriginal._triggeredBehaviours;
        _timer = _timeBetweenTriggering.rand;
    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        if(!_isActivated)
        {
            _isActivated = true;
            _timer = _timeBetweenTriggering.rand;
        }

        //foreach (var behaviour in _triggeredBehaviours)
        //{
        //    if (EnemyControl.BehaviourManager.GetBehaviour(behaviour).IsActive)
        //        return;
        //}
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _timer = _timeBetweenTriggering.rand;
            //Activate behaviours
            foreach (var behaviour in _triggeredBehaviours)
                EnemyControl.BehaviourManager.ActivateBehaviour(behaviour);
        }
    }
    public override void KillBehaviour()
    {
        base.KillBehaviour();
        _isActivated = false;
    }

}
