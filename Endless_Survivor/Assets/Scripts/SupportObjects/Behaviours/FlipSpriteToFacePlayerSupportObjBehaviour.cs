using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSpriteToFacePlayerSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] bool _originalFacingLeft;
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var flipSpriteOriginal = original as FlipSpriteToFacePlayerSupportObjBehaviour;
        _originalFacingLeft = flipSpriteOriginal._originalFacingLeft;
        OnUpdate += FlipSprite;

    }
    void FlipSprite()
    {
        Vector2 direction = PlayerControl.pc.transform.position - ObjControl.transform.position;
        bool transformFacingLeft = direction.x < 0;
        ObjControl.Renderers.ForEach(renderer => renderer.flipX =  transformFacingLeft != _originalFacingLeft);
    }
}
