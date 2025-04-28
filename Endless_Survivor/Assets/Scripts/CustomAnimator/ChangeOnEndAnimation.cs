using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChangeOnEndAnimation : CustomAnimation
{
    [SerializeField] string _exitAnimationName;

    public ChangeOnEndAnimation(ChangeOnEndAnimation original = null, string exitAnimationName = "") : base(original)
    {
        _exitAnimationName = original._exitAnimationName;
        OnAnimationEnd += ChangeAnimation;
    }

    public void ChangeAnimation(CustomAnimator customAnimator)
    {
        customAnimator.ChangeAnim(_exitAnimationName);
    }
}
