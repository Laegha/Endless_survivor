using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBehaviourManager : MonoBehaviour
{
    List<EnemyBehaviour> _behaviours = new List<EnemyBehaviour>();
    List<EnemyBehaviour> _activeBehaviours = new List<EnemyBehaviour>();
    EnemyControl _enemyControl;
    bool _isStunned;
    public List<EnemyBehaviour> Behaviours { get { return _behaviours; } }
    public bool IsStunned { get { return _isStunned; } set { _isStunned = value; } }

    private void Awake()
    {
        _enemyControl = transform.root.GetComponentInChildren<EnemyControl>();
        
    }
    private void Start()
    {
        _behaviours.ForEach(behaviour => behaviour.Start());
        _enemyControl.EnemyHP.OnDamageTaken += OnDamaged;
        _enemyControl.EnemyHP.OnDeath += OnDeath;
    }

    void Update()
    {
        if (_isStunned)
            return;
        _behaviours.ForEach(behaviour => behaviour.PassiveUpdate());
        List<EnemyBehaviour> activeBehaviours = new List<EnemyBehaviour>(_activeBehaviours);
        activeBehaviours.ForEach(behaviour => behaviour.ActiveUpdate());
    }

    void OnDamaged()
    {
        _behaviours.ForEach(behaviour => behaviour.OnDamaged());
    }
    void OnDeath(EnemyControl enemyControl)
    {
        _behaviours.ForEach(behaviour => behaviour.OnDeath());

    }
    public void AddBehaviour(EnemyBehaviour newBehaviour)
    {
        EnemyBehaviour addedBehaviour = (EnemyBehaviour)Activator.CreateInstance(newBehaviour.GetType());
        addedBehaviour.Initialize(newBehaviour, _enemyControl);
        _behaviours.Add(addedBehaviour);
    }

    public void RewriteAllOverrides()
    {
        foreach(EnemyBehaviour behaviour in _behaviours)
            behaviour.RewriteOverrides(_behaviours); 
    }
    public bool ActivateBehaviour(string newBehaviourId)
    {
        return ActivateBehaviour(Behaviours.Find(x => x.BehaviourId == newBehaviourId));
    }
    public bool ActivateBehaviour(EnemyBehaviour newBehaviour)
    {
        if (_activeBehaviours.Contains(newBehaviour))
            return true;
        foreach(var behaviour in _activeBehaviours)
        {
            bool isOverridenByActiveBehaviour = behaviour.OverrideBehaviours.Contains(newBehaviour.BehaviourId);
            //print("Behaviour " + behaviour + "Overrides " + newBehaviour + ": " + isOverridenByActiveBehaviour);
            if (isOverridenByActiveBehaviour)
                return false;
        }
        _activeBehaviours.Add(newBehaviour);
        newBehaviour.IsActive = true;
        //newBehaviour.OnActivated();
        CheckOverrides(newBehaviour);
        return true;
    }
    void CheckOverrides(EnemyBehaviour addedBehaviour)
    {
        foreach(var behaviourId in addedBehaviour.OverrideBehaviours)
        {
            var overridenBehaviour = _activeBehaviours.Find(x => x.BehaviourId == behaviourId);
            if (overridenBehaviour != null)
                overridenBehaviour.KillBehaviour();
        }
    }
    public void KillBehaviour(EnemyBehaviour removedBehaviour)
    {
        _activeBehaviours.Remove(removedBehaviour);
        removedBehaviour.IsActive = false;
    }
    public EnemyBehaviour GetBehaviour(string behaviourId)
    {
        return Behaviours.Find(x => x.BehaviourId == behaviourId);
    }

}
