using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBehavioursOnHitEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] string[] _triggeredBehaviours;
    [SerializeField] int _hitsToTrigger;
    [SerializeField] bool _onlyCountWhileActive;
    int _recievedHits = 0;

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var triggerOnHitOriginal = original as TriggerBehavioursOnHitEnemyBehaviour;
        _triggeredBehaviours = triggerOnHitOriginal._triggeredBehaviours;
        _hitsToTrigger = triggerOnHitOriginal._hitsToTrigger;
        _onlyCountWhileActive = triggerOnHitOriginal._onlyCountWhileActive;
    }
    public override void OnDamaged()
    {
        base.OnDamaged();
        if (!IsActive)
            return;
        _recievedHits++;
        if (_recievedHits >= _hitsToTrigger)
        {
            foreach (var behaviourId in _triggeredBehaviours)
            {
                EnemyControl.BehaviourManager.ActivateBehaviour(behaviourId);
            }
        }
    }
}
