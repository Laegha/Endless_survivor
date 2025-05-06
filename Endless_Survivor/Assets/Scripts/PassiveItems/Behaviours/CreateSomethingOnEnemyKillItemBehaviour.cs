using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CreateSomethingOnEnemyKillItemBehaviour : CreateSomethingOnPositionItemBehaviour
{
    new public static bool isUsable => true;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        behaviourManager.onEnemyKilled += CreateSomethingOnEnemyPosition;

    }
    void CreateSomethingOnEnemyPosition(EnemyControl killedEnemy)
    {

    }
}
