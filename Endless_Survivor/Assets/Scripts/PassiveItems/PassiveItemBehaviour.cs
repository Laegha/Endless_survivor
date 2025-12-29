using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PassiveItemBehaviour
{
    public static int maxStacks => 0;
    [SerializeField] string _behaviourId;
    PassiveItemBehaviourManager _behaviourManager;
    public string BehaviourId { get {  return _behaviourId; } }
    public PassiveItemBehaviourManager BehaviourManager { get { return _behaviourManager; } }
    public virtual void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        _behaviourId = original._behaviourId;
        _behaviourManager = behaviourManager;
    }
    public virtual void Activate()
    {

    }
}
