using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBehaviourManager : MonoBehaviour
{
    List<EnemyBehaviour> _behaviours = new List<EnemyBehaviour>();
    List<EnemyBehaviour> _activeBehaviours = new List<EnemyBehaviour>();

    void Update()
    {
        _behaviours.ForEach(behaviour => behaviour.PassiveUpdate());
        List<EnemyBehaviour> activeBehaviours = new List<EnemyBehaviour>(_activeBehaviours);
        activeBehaviours.ForEach(behaviour => behaviour.ActiveUpdate());
    }

    public void AddBehaviour(EnemyBehaviour newBehaviour, EnemyControl enemyControl)
    {
        EnemyBehaviour addedBehaviour = (EnemyBehaviour)Activator.CreateInstance(newBehaviour.GetType());
        addedBehaviour.Initialize(newBehaviour, enemyControl);
        _behaviours.Add(addedBehaviour);
    }

    public void RewriteAllOverrides()
    {
        foreach(EnemyBehaviour behaviour in _behaviours)
            behaviour.RewriteOverrides(_behaviours); 
    }

    public bool ActivateBehaviour(EnemyBehaviour newBehaviour)
    {
        foreach(var behaviour in _activeBehaviours)
        {
            bool isOverridenByActiveBehaviour = behaviour.OverrideBehaviours.Contains(newBehaviour);
            print("Behaviour " + behaviour + "Overrides " + newBehaviour + ": " + isOverridenByActiveBehaviour);
            if (isOverridenByActiveBehaviour)
                return false;
        }
        _activeBehaviours.Add(newBehaviour);

        CheckOverrides(newBehaviour);
        return true;
    }
    void CheckOverrides(EnemyBehaviour addedBehaviour)
    {
        foreach(var behaviour in addedBehaviour.OverrideBehaviours)
        {
            bool overridesActiveBehaviour = _activeBehaviours.Contains(behaviour);
            if (overridesActiveBehaviour)
                behaviour.KillBehaviour();
        }
    }
    public void KillBehaviour(EnemyBehaviour removedBehaviour)
    {
        _activeBehaviours.Remove(removedBehaviour);
    }
}
