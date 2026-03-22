using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerBehavioursAtHPThresholdEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] string[] _triggeredBehaviours;
    [SerializeField] float _hpThreshold;
    [SerializeField] bool _triggerConstantly;

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var triggerAtHPOriginal = original as TriggerBehavioursAtHPThresholdEnemyBehaviour;
        _triggeredBehaviours = triggerAtHPOriginal._triggeredBehaviours;
        _hpThreshold = triggerAtHPOriginal._hpThreshold;
        _triggerConstantly =  triggerAtHPOriginal._triggerConstantly;
    }

    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        float currHPPercentage = EnemyControl.EnemyHP.RemainingHP * 100 / EnemyControl.EnemyHP.MaxHP;
        if(currHPPercentage <= _hpThreshold)
        {
            foreach(var behaviourId in _triggeredBehaviours)
            {
                EnemyControl.BehaviourManager.ActivateBehaviour(behaviourId);
            }
            if (!_triggerConstantly)
                KillBehaviour();
        }
    }
}
