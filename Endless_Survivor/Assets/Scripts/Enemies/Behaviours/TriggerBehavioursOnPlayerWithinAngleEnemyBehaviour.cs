using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggerBehavioursOnPlayerWithinAngleEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] string[] _triggeredBehaviours;
    [SerializeField] List<AngleWindow> _triggeringAngles;
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var triggerOnAngleOriginal = original as TriggerBehavioursOnPlayerWithinAngleEnemyBehaviour;
        _triggeredBehaviours = triggerOnAngleOriginal._triggeredBehaviours;
        _triggeringAngles = triggerOnAngleOriginal._triggeringAngles;
    }
    public override void PassiveUpdate()
    {
        base.PassiveUpdate();
        Vector2 vectorToPlayer = PlayerControl.pc.transform.position - EnemyControl.transform.position;
        vectorToPlayer = vectorToPlayer.normalized;
        float angle = Mathf.Atan2(vectorToPlayer.y, vectorToPlayer.x) * Mathf.Rad2Deg;
        if (_triggeringAngles.Any(window => window.IsAngleInWindow(angle)))
            EnemyControl.BehaviourManager.ActivateBehaviour(this);
    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        foreach(var behaviourId in _triggeredBehaviours)
            EnemyControl.BehaviourManager.ActivateBehaviour(behaviourId);

        KillBehaviour();

    }
}