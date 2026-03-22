using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerRandomBehaviourOverTimeWhileActiveEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] string[] _possibleBehaviours;
    [SerializeField] RandomBetweenTwoConstants _timeBetweenTriggers;

    float _currTriggerTime;
    float _timer;
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var triggerRandomOriginal = original as TriggerRandomBehaviourOverTimeWhileActiveEnemyBehaviour;
        _possibleBehaviours = triggerRandomOriginal._possibleBehaviours;
        _timeBetweenTriggers = triggerRandomOriginal._timeBetweenTriggers;
        _currTriggerTime = _timeBetweenTriggers.rand;
    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();

        _timer += Time.deltaTime;
        if (_timer < _currTriggerTime)
            return;

        _currTriggerTime = _timeBetweenTriggers.rand;
        _timer = 0;
        string triggeredBehaviourId = _possibleBehaviours[Random.Range(0, _possibleBehaviours.Length)];

        EnemyControl.BehaviourManager.ActivateBehaviour(triggeredBehaviourId);

    }
    public override void KillBehaviour()
    {
        base.KillBehaviour();
        _timer = 0;
        _currTriggerTime = _timeBetweenTriggers.rand;

    }
}
