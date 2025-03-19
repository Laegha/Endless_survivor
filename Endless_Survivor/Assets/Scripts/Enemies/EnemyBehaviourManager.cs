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
        _activeBehaviours.ForEach(behaviour => behaviour.ActiveUpdate());
    }

    public void AddBehaviour(EnemyBehaviour newBehaviour, EnemyControl enemyControl)
    {
        _behaviours.Add((EnemyBehaviour)Activator.CreateInstance(newBehaviour.GetType()));
        newBehaviour.Start(enemyControl);
    }

    public bool ActivateBehaviour(EnemyBehaviour newBehaviour)
    {
        foreach(var behaviour in _activeBehaviours)
        {
            bool isOverridenByActiveBehaviour = behaviour.OverrideBehaviours.Where(overrideBehaviour => overrideBehaviour.GetType() == newBehaviour.GetType()).Count() != 0;
            if(isOverridenByActiveBehaviour)
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
            //if()
        }
    }
}
