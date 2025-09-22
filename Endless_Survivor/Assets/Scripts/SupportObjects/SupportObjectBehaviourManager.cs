using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportObjectBehaviourManager : MonoBehaviour
{
    List<SupportObjectBehaviour> _supportObjBehaviours;

    public void SetBehaviours(List<SupportObjectBehaviour> originalBehaviours)
    {
        foreach(var behaviour in originalBehaviours)
        {
            var behaviourCopy = (SupportObjectBehaviour)Activator.CreateInstance(behaviour.GetType());
            _supportObjBehaviours.Add(behaviourCopy);
            behaviourCopy.Initiate(this, behaviour);
            behaviourCopy.OnStart?.Invoke();
        }
    }

    private void Update()
    {
        foreach( var behaviour in _supportObjBehaviours)
            behaviour.OnUpdate?.Invoke();
    }
    public void CollidedWithPlayer()
    {
        foreach(var behaviour in _supportObjBehaviours)
            behaviour.OnCollidedWithPlayer?.Invoke();
    }
    public void CollidedWithEnemy(EnemyControl collidedEnemy)
    {
        foreach (var behaviour in _supportObjBehaviours)
            behaviour.OnCollidedWithEnemy?.Invoke(collidedEnemy);
    }
}
