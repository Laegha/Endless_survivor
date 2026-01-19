using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CreateSupportObjWeaponInteraction : WeaponInteraction
{
    new public static int maxStacks => -1;
    [SerializeField] SupportObjectData _createdSupportObj;
    [SerializeField] Vector2 _offsetFromPlayer;
    [SerializeField] float _rotation;

    public override void Initialize(WeaponInteraction original, WeaponInteractionData interactionData, List<WeaponAttackManager> affectedWeapons)
    {
        base.Initialize(original, interactionData, affectedWeapons);
        var createSupportObjOriginal = original as CreateSupportObjWeaponInteraction;
        _createdSupportObj = createSupportObjOriginal._createdSupportObj;
        _offsetFromPlayer = createSupportObjOriginal._offsetFromPlayer;
        _rotation = createSupportObjOriginal._rotation;
    }

    public override void InteractionStart()
    {
        base.InteractionStart();
        Utility.GenerateSupportObj(_createdSupportObj, (Vector2)PlayerControl.pc.transform.position + _offsetFromPlayer, Quaternion.Euler(0,0, _rotation));
    }
}
