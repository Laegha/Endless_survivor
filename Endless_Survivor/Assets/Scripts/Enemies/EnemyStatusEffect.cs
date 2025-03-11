using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusEffect
{
    EnemyControl _affectedEnemyControl;

    public EnemyStatusEffect(EnemyControl affectedEnemyControl)
    {
        _affectedEnemyControl = affectedEnemyControl;
    }

    public virtual void Start(){ }
    public virtual void Update() { }
}
