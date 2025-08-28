using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RandomIdleAnimation : CustomAnimation
{
    [SerializeField] int _animationWeight = 1;
    public int AnimationWeight { get { return _animationWeight; } }
}
