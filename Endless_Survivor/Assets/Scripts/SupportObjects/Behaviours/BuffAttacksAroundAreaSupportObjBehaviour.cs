using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffAttacksAroundAreaSupportObjBehaviour : UseAreaAroundSupportObjBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] float _attackDamageMultiplierIncrease = 2;
    //[SerializeField] AttackGfxInterface _attackGfxChange;
    [SerializeField] GameObject _buffedAttackGfxIndicatorPrefab;
    List<TransformFollowHandler> _indicatorsFollowHandlers = new();
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var buffAttackOriginal = original as BuffAttacksAroundAreaSupportObjBehaviour;
        _attackDamageMultiplierIncrease = buffAttackOriginal._attackDamageMultiplierIncrease;
        _buffedAttackGfxIndicatorPrefab = buffAttackOriginal._buffedAttackGfxIndicatorPrefab;
        OnObjEnterArea += BuffAttack;
        OnUpdate += UpdateFollowHandlers;
    }
    void BuffAttack(GameObject enteredObj)
    {
        Attack attack = enteredObj.GetComponent<Attack>();
        if (attack == null)
            return;
        attack.AttackDamageMultiplier += _attackDamageMultiplierIncrease;
        var buffIndicator = GameObject.Instantiate(_buffedAttackGfxIndicatorPrefab);
        TransformFollowHandler buffIndicatorFollowHandler = new(buffIndicator.transform, attack.transform, true, false);
        _indicatorsFollowHandlers.Add(buffIndicatorFollowHandler);
    }
    void UpdateFollowHandlers()
    {
        var indicatorsFollowHandlersCopy = new List<TransformFollowHandler>(_indicatorsFollowHandlers);
        foreach(TransformFollowHandler handler in indicatorsFollowHandlersCopy)
        {
            if(handler.parent == null)
            {
                GameObject.Destroy(handler.child.gameObject);
                _indicatorsFollowHandlers.Remove(handler);
                continue;
            }
            handler.Update();
        }
    }
}
