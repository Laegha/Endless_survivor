using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimOnAreaEnterSupportObjBehaviour : UseAreaAroundSupportObjBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] CustomAnimation _animation;
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var playAnimOriginal = original as PlayAnimOnAreaEnterSupportObjBehaviour;
        _animation = playAnimOriginal._animation;
        ObjControl.Animator.AddAnimations(new() { _animation });

        OnObjEnterArea += PlayAnim;
    }

    void PlayAnim(GameObject placeholder)
    {
        ObjControl.Animator.ChangeAnim(_animation.AnimationName);
    }
}
