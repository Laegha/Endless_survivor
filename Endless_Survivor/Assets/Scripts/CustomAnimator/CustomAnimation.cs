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
    
    List<AnimationEvent> _events = new List<AnimationEvent>();

    public string AnimationName {  get { return _animationName; } }
    public Sprite[] Frames {  get { return _frames; } }
    public float FramesPerSecond {  get { return _framesPerSecond; } }
    public int Priority {  get { return _priority; } }
    public Action<CustomAnimator> OnAnimationEnd { get { return _onAnimationEnd; } set { _onAnimationEnd = value; } }
    public List<AnimationEvent> Events { get { return _events; } }

    public CustomAnimation(CustomAnimation original = null, string animationName = "", Sprite[] frames = null, float framesPerSecond = 1, int priority = 0, Action<CustomAnimator> onAnimationEnd = null, List<AnimationEvent> events = null)
    {
        if(original != null)
        {
            _animationName = original._animationName;
            _frames = original._frames;
            _framesPerSecond = original._framesPerSecond;
            _priority = original._priority;
            //_onAnimationEnd = original._onAnimationEnd;
            if(original._onAnimationEnd != null) 
                foreach (var action in original._onAnimationEnd.GetInvocationList())
                    _onAnimationEnd += (Action<CustomAnimator>)action;
        
            if(original._events != null)
                foreach(var animEvent in original._events)
                    _events.Add(new AnimationEvent(animEvent));
            return;
        }
        _animationName = animationName;
        _frames = frames;
        _framesPerSecond = framesPerSecond;
        _priority = priority;
        _onAnimationEnd = onAnimationEnd;
        _events = events != null ? events : new List<AnimationEvent>();
    }
}
