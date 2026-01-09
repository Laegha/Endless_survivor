using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBehavioursAtDistance : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeReference] List<string> _behavioursToTrigger = new List<string>();
    [SerializeField] float _distanceToTrigger;
    Transform _player;

    public List<string> BehavioursToTrigger {  get { return _behavioursToTrigger; } }
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        TriggerBehavioursAtDistance originalTriggerBehavioursAtDistance = original as TriggerBehavioursAtDistance;
        _behavioursToTrigger = originalTriggerBehavioursAtDistance._behavioursToTrigger;
        _distanceToTrigger = originalTriggerBehavioursAtDistance._distanceToTrigger;
        _player = GameObject.FindObjectOfType<PlayerControl>().transform;
    }
    public override void PassiveUpdate()
    {
        base.PassiveUpdate();
        if(IsActive)
            return;
        float distance = Vector2.Distance(EnemyControl.transform.position, _player.position);
        if(distance <= _distanceToTrigger)
        {
            foreach (var behaviour in _behavioursToTrigger)
                EnemyControl.BehaviourManager.ActivateBehaviour(behaviour);
            EnemyControl.BehaviourManager.ActivateBehaviour(this);
        }
    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        foreach (var behaviour in _behavioursToTrigger)
        {
            if (EnemyControl.BehaviourManager.GetBehaviour(behaviour).IsActive)
                return;
        }
        KillBehaviour();
    }
}
