using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class AddStatusWithEnoughStacksStatusEffect : EnemyStatusEffect
{
    new public static bool isUsable => true;
    [SerializeField] int _neededStacksToTrigger;
    [SerializeField] EnemyStatusEffectData _statusEffectAdded;

    bool _thisEnded = false;

    public override void Initialize(EnemyControl affectedEnemyControl, EnemyStatusEffect original)
    {
        base.Initialize(affectedEnemyControl, original);
        var addStatusWithStacksOriginal = original as AddStatusWithEnoughStacksStatusEffect;
        _neededStacksToTrigger = addStatusWithStacksOriginal._neededStacksToTrigger;
        _statusEffectAdded = addStatusWithStacksOriginal._statusEffectAdded;
        EndCondition = CheckCondition;
    }
    public override void Start()
    {
        base.Start();
        
        var stackedEffects = AffectedEnemyControl.StatusEffectManager.CurrentEffects.Where(x => x.effectData == ThisGroup.effectData).ToArray();
        int currStacks = stackedEffects.Length;
        if (currStacks < _neededStacksToTrigger -1)// -1 because by this point, this behaviour hasn't been added yet, but then when it is added, it ends inmediately
            return;
        AffectedEnemyControl.StatusEffectManager.AddEffects(_statusEffectAdded.StatusEffects, _statusEffectAdded);
    }
    bool CheckCondition()
    {
        if (_thisEnded)
            return true;
        var stackedEffects = AffectedEnemyControl.StatusEffectManager.CurrentEffects.Where(x => x.effectData == ThisGroup.effectData).ToArray();
        //return stackedEffects.Length >= _neededStacksToTrigger;
        //first we check for the negative and simple case where every instance of the effect ends
        if (stackedEffects.Length < _neededStacksToTrigger)
            return false;
        else if (stackedEffects.Length == _neededStacksToTrigger)
            _thisEnded = true;
        //if there are more effects than needed, end only the first needed. if this isn't one of the first, it isn't ended
        for(int i = 0; i < _neededStacksToTrigger; i ++)
        {
            if (stackedEffects[i] == ThisGroup)
                _thisEnded = true;
        }
        return false;
    }
}
