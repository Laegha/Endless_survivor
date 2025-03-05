using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChangeOnEndAnimation : CustomAnimation
{
    [SerializeField] string _exitAnimationName;

    public ChangeOnEndAnimation()
    {
        OnAnimationEnd += ChangeAnimation;
    }

    public void ChangeAnimation(CustomAnimator customAnimator)
    {
        customAnimator.ChangeAnim(_exitAnimationName);
    }
}
