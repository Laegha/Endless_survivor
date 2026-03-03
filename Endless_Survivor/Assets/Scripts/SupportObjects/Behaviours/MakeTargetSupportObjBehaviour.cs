using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MakeTargetSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] int _targetPriority = 1;
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var makeTargetOriginal = original as MakeTargetSupportObjBehaviour;
        _targetPriority = makeTargetOriginal._targetPriority;
        WeaponAim.SharedAttackTargets.Add(new(ObjControl.gameObject, _targetPriority));
    }
}
