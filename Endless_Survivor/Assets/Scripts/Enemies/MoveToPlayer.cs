using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPlayer : MoveToTarget
{
    EnemyControl _enemyControl;
    public EnemyControl EnemyControl { set {  _enemyControl = value; } }
    void Start()
    {
        TargetTag = "Player";
    }
    public override void Update()
    {
        base.Update();
        if (_enemyControl.CustomAnimator.CurrAnim.AnimationName != "Moving")
            _enemyControl.CustomAnimator.ChangeAnim("Moving");
    }
}
