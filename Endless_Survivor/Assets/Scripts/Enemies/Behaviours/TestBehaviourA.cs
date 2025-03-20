using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBehaviourA : EnemyBehaviour
{
    public override void PassiveUpdate()
    {
        base.PassiveUpdate();
        if(Input.GetKeyDown(KeyCode.A) && !IsActive)
            EnemyControl.BehaviourManager.ActivateBehaviour(this);
    }

    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        Debug.Log("Test A is currently active");
        Debug.Log("Test A overrides: " + OverrideBehaviours.Count);
        if (Input.GetKeyDown(KeyCode.Q))
            KillBehaviour();
    }

    public override void KillBehaviour()
    {
        Debug.Log("Test A has been killed");
        base.KillBehaviour();
    }
}
