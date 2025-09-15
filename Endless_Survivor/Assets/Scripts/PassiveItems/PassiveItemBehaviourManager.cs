using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItemBehaviourManager
{
    public Action onPicked;
    public Action onUpdate;
    public Action<Weapon> onAttack;//make a system for attacks, so passive items can modify them
    public Action onDamageRecieved;
    public Action<EnemyControl> onEnemyHit;
    public Action<EnemyControl> onEnemyKilled;
    public Action onWaveChanged;

    List<PassiveItemBehaviour> _itemBehaviours = new();
    public void CopyBehaviours(List<PassiveItemBehaviour> itemBehaviours)
    {
        foreach (PassiveItemBehaviour behaviour in itemBehaviours)
        {
            PassiveItemBehaviour addedBehaviour = (PassiveItemBehaviour)Activator.CreateInstance(behaviour.GetType());
            addedBehaviour.CopyValues(behaviour, this);
            _itemBehaviours.Add(addedBehaviour);
        }
    }
}
