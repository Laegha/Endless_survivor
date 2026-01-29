using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBehavioursDuringTimeEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] string[] _behavioursToTrigger;
    [SerializeField] RandomBetweenTwoConstants _duration;

    float _timer;
    float _currDuration;

    bool _activated = false;

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var triggerDuringTimeOriginal = original as TriggerBehavioursDuringTimeEnemyBehaviour;
        _behavioursToTrigger = triggerDuringTimeOriginal._behavioursToTrigger;
        _duration = triggerDuringTimeOriginal._duration;
        _currDuration = _duration.rand;
    }

    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        if(!_activated)
        {
            _activated = true;
            _currDuration = _duration.rand;
            _timer = 0;
            foreach (var behaviourId in _behavioursToTrigger)
            {
                EnemyControl.BehaviourManager.ActivateBehaviour(behaviourId);
            }
        }
        _timer += Time.deltaTime;
        if(_timer > _currDuration)
        {
            foreach(var behaviourId in _behavioursToTrigger)
            {
                var behaviour = EnemyControl.BehaviourManager.GetBehaviour(behaviourId);
                behaviour.KillBehaviour();
            }
            KillBehaviour();
        }
    }
    public override void KillBehaviour()
    {
        base.KillBehaviour();
        _activated = false;
    }
}
