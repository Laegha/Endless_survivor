using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBehavioursAfterTimeOnActiveEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] List<string> _triggeredBehaviours;
    [SerializeField] float _timeToTrigger;
    [SerializeField] bool _triggerOnStart;

    float _timer;

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var triggerAfterTimeOriginal = original as TriggerBehavioursAfterTimeOnActiveEnemyBehaviour;
        _triggeredBehaviours = triggerAfterTimeOriginal._triggeredBehaviours;
        _timeToTrigger = triggerAfterTimeOriginal._timeToTrigger;
        _triggerOnStart = triggerAfterTimeOriginal._triggerOnStart;
    }
    public override void Start()
    {
        base.Start();
        if(_triggerOnStart)
            EnemyControl.BehaviourManager.ActivateBehaviour(this);
    }
    public override void ActiveUpdate()
    {
        base.PassiveUpdate();
        _timer += Time.deltaTime;
        if (_timer < _timeToTrigger)
            return;
        _timer = 0;
        foreach (var triggeredBehaviour in _triggeredBehaviours)
            EnemyControl.BehaviourManager.ActivateBehaviour(triggeredBehaviour);
        KillBehaviour();
    }

}
