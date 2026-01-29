using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRandomBehaviourAfterTimeEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] string[] _possibleBehaviours;
    [SerializeField] RandomBetweenTwoConstants _timeBetweenTriggers;

    EnemyBehaviour _currTriggeredBehaviour;
    float _currTriggerTime;
    float _timer;
    bool _activated;
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var triggerRandomOriginal = original as TriggerRandomBehaviourAfterTimeEnemyBehaviour;
        _possibleBehaviours = triggerRandomOriginal._possibleBehaviours;
        _timeBetweenTriggers = triggerRandomOriginal._timeBetweenTriggers;
        _currTriggerTime = _timeBetweenTriggers.rand;
    }

    public override void PassiveUpdate()
    {
        base.PassiveUpdate();

        if (_activated)
            return;
        _timer += Time.deltaTime;
        if (_timer < _currTriggerTime)
            return;

        _timer = 0;
        _activated = true;
        string triggeredBehaviourId = _possibleBehaviours[Random.Range(0, _possibleBehaviours.Length)];
        _currTriggeredBehaviour = EnemyControl.BehaviourManager.GetBehaviour(triggeredBehaviourId);
        
        EnemyControl.BehaviourManager.ActivateBehaviour(triggeredBehaviourId);

        EnemyControl.BehaviourManager.ActivateBehaviour(this);
    }

    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        if (_currTriggeredBehaviour != null && _currTriggeredBehaviour.IsActive || !_activated)
            return;
        KillBehaviour();
    }
    public override void KillBehaviour()
    {
        base.KillBehaviour();
        _activated = false;
        _currTriggerTime = _timeBetweenTriggers.rand;

    }
}
