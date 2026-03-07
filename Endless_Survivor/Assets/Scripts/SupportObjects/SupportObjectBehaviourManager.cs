using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportObjectBehaviourManager : MonoBehaviour
{
    [SerializeField] SupportObjectControl _objControl;
    List<SupportObjectBehaviour> _supportObjBehaviours = new();
    SupportObjectData _supportObjData;

    public List<SupportObjectBehaviour> Behaviours { get { return _supportObjBehaviours; } }
    public SupportObjectData SupportObjData {  get { return _supportObjData; } }

    public void SetBehaviours(List<SupportObjectBehaviour> originalBehaviours, SupportObjectData supportObjectData)
    {
        _supportObjData = supportObjectData;
        foreach(var behaviour in originalBehaviours)
        {
            var behaviourCopy = (SupportObjectBehaviour)Activator.CreateInstance(behaviour.GetType());
            _supportObjBehaviours.Add(behaviourCopy);
            behaviourCopy.Initiate(_objControl, behaviour);
        }
        foreach(var behaviour in _supportObjBehaviours)
            behaviour.OnStart?.Invoke();
    }

    private void Update()
    {
        foreach(var behaviour in _supportObjBehaviours)
            behaviour.OnUpdate?.Invoke();
    }
    private void LateUpdate()
    {

        foreach (var behaviour in _supportObjBehaviours)
            behaviour.OnLateUpdate?.Invoke();
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
    public void Destroyed()
    {
        foreach (var behaviour in _supportObjBehaviours)
            behaviour.OnDestroyed?.Invoke();
    }
}
