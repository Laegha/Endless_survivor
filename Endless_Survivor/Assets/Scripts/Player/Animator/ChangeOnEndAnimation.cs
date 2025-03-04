using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOnEndAnimation : CustomAnimation
{
    [SerializeField] string _exitAnimationName;

    public override void AnimationEnded(CustomAnimator customAnimator)
    {
        base.AnimationEnded(customAnimator);
        customAnimator.ChangeAnim(_exitAnimationName);
    }
}
