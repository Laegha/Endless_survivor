using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBehavioursAtDistance : EnemyBehaviour
{
    [SerializeReference] List<EnemyBehaviour> _behavioursToTrigger = new List<EnemyBehaviour>();
    [SerializeField] float _distanceToTrigger;
    Transform _player;

    public List<EnemyBehaviour> BehavioursToTrigger {  get { return _behavioursToTrigger; } }
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        TriggerBehavioursAtDistance originalTriggerBehavioursAtDistance = original as TriggerBehavioursAtDistance;
        _behavioursToTrigger = originalTriggerBehavioursAtDistance._behavioursToTrigger;
        _distanceToTrigger = originalTriggerBehavioursAtDistance._distanceToTrigger;
        _player = GameObject.FindObjectOfType<PlayerControl>().transform;
    }
    public override void Start()
    {
        base.Start();
        List<EnemyBehaviour> runtimeBehavioursToTrigger = new List<EnemyBehaviour>();
        foreach (var assetBehaviour in _behavioursToTrigger)
        {
            var runtimeBehaviour = EnemyControl.BehaviourManager.Behaviours.Find(behaviour => behaviour.GetType() == assetBehaviour.GetType());
            if (runtimeBehaviour == null)
            {
                Debug.LogError("Trying to trigger at distance the behaviour " + assetBehaviour.GetType() + " that isn't in the gameobject (this should never happen)");
                continue;
            }
            runtimeBehavioursToTrigger.Add(runtimeBehaviour);
        }
        _behavioursToTrigger = runtimeBehavioursToTrigger;
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
            if (behaviour.IsActive)
                return;
        }
        KillBehaviour();
    }
}
