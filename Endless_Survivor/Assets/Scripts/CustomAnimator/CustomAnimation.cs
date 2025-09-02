using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class CustomAnimation
{
    [SerializeField] string _animationName;
    [SerializeField] Sprite[] _frames;
    [SerializeField] float _framesPerSecond;
    [Tooltip("The higher the number the more priority the animation has")]
    [SerializeField] int _priority;

    //Action<CustomAnimator> _onAnimationEnd;
    CustomAnimator _animator;
    List<AnimationEvent> _events = new List<AnimationEvent>();

    public string AnimationName {  get { return _animationName; } }
    public Sprite[] Frames {  get { return _frames; } }
    public float AnimDuration { get { return _frames.Length / _framesPerSecond; } }
    public float FramesPerSecond {  get { return _framesPerSecond; } }
    public int Priority {  get { return _priority; } }
    //public Action<CustomAnimator> OnAnimationEnd { get { return _onAnimationEnd; } set { _onAnimationEnd = value; } }
    public AnimationEvent OnAnimEnd
    {
        get
        {
            var endEvent = _events.Find(x => x.frameIndex == _frames.Length-1);

            return endEvent;
        }
    }
    public List<AnimationEvent> Events { get { return _events; } }
    public CustomAnimator Animator { get { return _animator; } }
    public CustomAnimation(CustomAnimator animator, CustomAnimation original = null)
    {
        _animator = animator;
        if (original == null)
            return;

        _animationName = original._animationName;
        _frames = original._frames;
        _framesPerSecond = original._framesPerSecond;
        _priority = original._priority;
        //if(original._onAnimationEnd != null) 
        //foreach (var action in original._onAnimationEnd.GetInvocationList())
        //_onAnimationEnd += (Action<CustomAnimator>)action;

        if (original._events != null)
            foreach (var animEvent in original._events)
            {
                _events.Add(new AnimationEvent(animEvent));
            }
    }
    public CustomAnimation(CustomAnimator animator, string animationName = "", Sprite[] frames = null, float framesPerSecond = 1, int priority = 0, List<AnimationEvent> events = null)
    {
        _animator = animator;
        _animationName = animationName;
        _frames = frames;
        _framesPerSecond = framesPerSecond;
        _priority = priority;
        _events = events != null ? events : new List<AnimationEvent>();
    }
}
