using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChangeOnEndAnimation : CustomAnimation
{
    [SerializeField] string _exitAnimationName;

    public ChangeOnEndAnimation(CustomAnimator animator, ChangeOnEndAnimation original = null) : base(animator,original)
    {
        _exitAnimationName = original._exitAnimationName;
        //Events.Add(new(null, Frames.Length - 1, ChangeAnimation));
        //OnAnimationEnd += ChangeAnimation;
        Events.Add(new(null, Frames.Length - 1, ChangeAnimation));
    }

    public void ChangeAnimation()
    {
        Animator.ChangeAnim(_exitAnimationName, true);
    }
}
