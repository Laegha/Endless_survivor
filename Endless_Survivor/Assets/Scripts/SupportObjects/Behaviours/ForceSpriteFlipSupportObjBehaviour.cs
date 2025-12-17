using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceSpriteFlipSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] bool _forceXFlip;
    [SerializeField] bool _flipX;
    [SerializeField] bool _forceYFlip;
    [SerializeField] bool _flipY;

    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var forceFlipOriginal = original as ForceSpriteFlipSupportObjBehaviour;
        _forceXFlip = forceFlipOriginal._forceXFlip;
        _flipX = forceFlipOriginal._flipX;
        _forceYFlip = forceFlipOriginal._forceYFlip;
        _flipY = forceFlipOriginal._flipY;

        OnLateUpdate += ForceFlip;
    }

    void ForceFlip()
    {
        var children = ObjControl.GetComponentsInChildren<SpriteRenderer>();
        foreach (var child in children)
        {
            if(_forceXFlip)
                child.flipX = _flipX;
            if(_forceYFlip)
                child.flipY = _flipY;

        }

    }
}
