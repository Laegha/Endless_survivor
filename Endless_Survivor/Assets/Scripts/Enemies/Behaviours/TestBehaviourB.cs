using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBehaviourB : EnemyBehaviour
{
    public override void PassiveUpdate()
    {
        base.PassiveUpdate();
        if (Input.GetKeyDown(KeyCode.B) && !IsActive)
            EnemyControl.BehaviourManager.ActivateBehaviour(this);
    }

    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        Debug.Log("Test B is currently active");
        if (Input.GetKeyDown(KeyCode.B))
            KillBehaviour();
    }

    public override void KillBehaviour()
    {
        Debug.Log("Test B has been killed");
        base.KillBehaviour();
    }
}
