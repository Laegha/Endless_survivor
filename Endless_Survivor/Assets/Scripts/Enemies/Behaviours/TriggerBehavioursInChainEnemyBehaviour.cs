using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBehavioursInChainEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] string[] _chainedBehavioursInOrder;
    List<EnemyBehaviour> _chainedBehaviours = new();
    int _currBehaviourId = -1;
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var triggerInChainOriginal = original as TriggerBehavioursInChainEnemyBehaviour;
        _chainedBehavioursInOrder = triggerInChainOriginal._chainedBehavioursInOrder;
    }
    public override void Start()
    {
        base.Start();
        foreach(var behaviourId in _chainedBehavioursInOrder)
        {
            _chainedBehaviours.Add(EnemyControl.BehaviourManager.GetBehaviour(behaviourId));
        }
        Debug.Log("Chained behaviour " + _chainedBehavioursInOrder.Length);
    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        if (_currBehaviourId > _chainedBehavioursInOrder.Length)//this bs is to get avoid killing itself twice since there's a frame of delay between the last update and kill
            return;

        if (_currBehaviourId >= 0 && _chainedBehaviours[_currBehaviourId].IsActive)
            return;

        _currBehaviourId++;
        if (_currBehaviourId == _chainedBehavioursInOrder.Length)
        {
            KillBehaviour();
            _currBehaviourId++;
            return;
        }
        EnemyControl.BehaviourManager.ActivateBehaviour(_chainedBehavioursInOrder[_currBehaviourId]);

    }

    public override void KillBehaviour()
    {
        base.KillBehaviour();
        if(_currBehaviourId >= _chainedBehavioursInOrder.Length)
            _currBehaviourId = _chainedBehavioursInOrder.Length - 1;
        if(_chainedBehaviours[_currBehaviourId].IsActive)
            _chainedBehaviours[_currBehaviourId].KillBehaviour();
        GameManager.gm.DelayActionAFrame(() => _currBehaviourId = -1, () => EnemyControl == null);
    }
}
