using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBehavioursAtDistanceWhileActiveEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeReference] List<string> _behavioursToTrigger = new List<string>();
    [SerializeField] float _distanceToTrigger;
    [SerializeField] bool _triggerWhenFar;
    [SerializeField] List<string> _behavioursToWaitBeforeTriggeringAgain;
    Transform _player;

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        TriggerBehavioursAtDistanceWhileActiveEnemyBehaviour originalTriggerBehavioursAtDistance = original as TriggerBehavioursAtDistanceWhileActiveEnemyBehaviour;
        _behavioursToTrigger = originalTriggerBehavioursAtDistance._behavioursToTrigger;
        _distanceToTrigger = originalTriggerBehavioursAtDistance._distanceToTrigger;
        _triggerWhenFar = originalTriggerBehavioursAtDistance._triggerWhenFar;
        _behavioursToWaitBeforeTriggeringAgain = new(originalTriggerBehavioursAtDistance._behavioursToWaitBeforeTriggeringAgain);
        foreach(string behaviour in _behavioursToTrigger)
        {
            if (_behavioursToWaitBeforeTriggeringAgain.Contains(behaviour))
                continue;
            _behavioursToWaitBeforeTriggeringAgain.Add(behaviour);
        }

        _player = GameObject.FindObjectOfType<PlayerControl>().transform;
    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        foreach (var behaviour in _behavioursToWaitBeforeTriggeringAgain)
        {
            if (EnemyControl.BehaviourManager.GetBehaviour(behaviour).IsActive)
                return;
        }
        float distance = Vector2.Distance(EnemyControl.transform.position, _player.position);
        if ((!_triggerWhenFar && distance <= _distanceToTrigger) || (_triggerWhenFar && distance >= _distanceToTrigger))
        {
            foreach (var behaviour in _behavioursToTrigger)
                EnemyControl.BehaviourManager.ActivateBehaviour(behaviour);
            EnemyControl.BehaviourManager.ActivateBehaviour(this);
        }
    }
}
