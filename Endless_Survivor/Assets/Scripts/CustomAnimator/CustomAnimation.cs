using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomAnimation
{
    [SerializeField] string _animationName;
    [SerializeField] Sprite[] _frames;
    [SerializeField] float _framesPerSecond;
    [Tooltip("The higher the number the more priority the animation has")]
    [SerializeField] int _priority;
    Action<CustomAnimator> _onAnimationEnd;

    public string AnimationName {  get { return _animationName; } }
    public Sprite[] Frames {  get { return _frames; } }
    public float FramesPerSecond {  get { return _framesPerSecond; } }
    public int Priority {  get { return _priority; } }
    public Action<CustomAnimator> OnAnimationEnd { get { return _onAnimationEnd; } set { _onAnimationEnd = value; } }

}
