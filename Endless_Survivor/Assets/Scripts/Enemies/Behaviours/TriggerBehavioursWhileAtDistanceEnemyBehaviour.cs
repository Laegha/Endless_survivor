using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBehavioursWhileAtDistanceEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeReference] List<string> _behavioursToTrigger = new List<string>();
    [SerializeField] float _distanceToTrigger;
    Transform _player;
    bool _isActivated = false;

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var originalTriggerBehavioursWhileAtDistance = original as TriggerBehavioursWhileAtDistanceEnemyBehaviour;
        _behavioursToTrigger = originalTriggerBehavioursWhileAtDistance._behavioursToTrigger;
        _distanceToTrigger = originalTriggerBehavioursWhileAtDistance._distanceToTrigger;
        _player = PlayerControl.pc.transform;
    }
    public override void PassiveUpdate()
    {
        base.PassiveUpdate();
        if (IsActive)
            return;
        float distance = Vector2.Distance(EnemyControl.transform.position, _player.position);
        if (distance <= _distanceToTrigger)
        {
            foreach (var behaviour in _behavioursToTrigger)
                EnemyControl.BehaviourManager.ActivateBehaviour(behaviour);
            EnemyControl.BehaviourManager.ActivateBehaviour(this);
            _isActivated = true;
        }
    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        float distance = Vector2.Distance(EnemyControl.transform.position, _player.position);
        if (distance <= _distanceToTrigger || !_isActivated)
            return;
        _isActivated = false;
        foreach (var behaviour in _behavioursToTrigger)
            EnemyControl.BehaviourManager.GetBehaviour(behaviour).KillBehaviour();
        KillBehaviour();
    }
}
