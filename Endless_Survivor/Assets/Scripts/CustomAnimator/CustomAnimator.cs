using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class CustomAnimator : MonoBehaviour
{
    List<CustomAnimation> _animations = new List<CustomAnimation>();
    CustomAnimation _currAnim;
    int _currFrameIndex;
    [SerializeField] SpriteRenderer _spriteRenderer;
    float _animTimer = 0;
    int _currAnimPriority;

    public CustomAnimation CurrAnim { get {  return _currAnim; } set { _currAnim = value; } }
    public int CurrFrameIndex { get { return _currFrameIndex; } set { _currFrameIndex = value; } }
    public List<CustomAnimation> Animations{ get {  return _animations; } set { _animations = value; } }
    public SpriteRenderer Renderer { get { return _spriteRenderer; } }

    public virtual void AddAnimations(List<CustomAnimation> newAnimations) 
    {
        foreach (var anim in newAnimations)
        {
            if (_animations.Any(x => x.AnimationName == anim.AnimationName))
            {
                Debug.LogError("ANIMATOR ERROR: trying to add an animation with the name " + anim.AnimationName + " but there already is one");
                continue;
            }
            var newAnim = Activator.CreateInstance(anim.GetType(),this, anim) as CustomAnimation;
            _animations.Add(newAnim);
        }

    }

    void Update()
    {
        if (_currAnim == null)
            return;
        _animTimer -= Time.deltaTime;
        if(_animTimer <= 0)
        {
            NextFrame();
        }
    }

    public void NextFrame()
    {
        _currFrameIndex++;
        if(_currFrameIndex >= _currAnim.Frames.Length)
        {
            _currFrameIndex = 0;
            if (_currAnim.EndAnimationOnEnd)
                EndAnimation(_currAnim);
        }
        _spriteRenderer.sprite = _currAnim.Frames[_currFrameIndex];
        _currAnim.Events.Where(animEvent => animEvent.frameIndex == _currFrameIndex)?.ToList().ForEach(animEvent => animEvent.frameAction?.Invoke());//this doesn't work if the event is set for the frame 0, since the event is called after the frame changed
        _animTimer = 1 / _currAnim.FramesPerSecond;
    }

    public void ChangeAnim(CustomAnimation animation) => ChangeAnim(animation.AnimationName);
    public virtual void ChangeAnim(string animName, bool overridePriority = false, bool resetSameAnim = false)
    {
        CustomAnimation newAnimation = _animations.Find(anim => anim.AnimationName == animName);
        if(newAnimation == null)
        {
            Debug.LogError("ERROR: Animation not found: " +  animName + " on animator " + gameObject.name);
            return;
        }
        if(_currAnim != null && _currAnimPriority > newAnimation.Priority && !overridePriority || _currAnim == newAnimation && !resetSameAnim)
            return;
        _currAnim = newAnimation;
        _currAnimPriority = newAnimation.Priority;
        _currFrameIndex = -1;
        NextFrame();
    }
    public virtual void ChangeAnimButKeepFrame(string animName, bool overridePriority = false, bool resetSameAnim = false)
    {
        CustomAnimation newAnimation = _animations.Find(anim => anim.AnimationName == animName);
        if (newAnimation == null)
        {
            Debug.LogError("ERROR: Animation not found: " + animName + " on animator " + gameObject.name);
            return;
        }
        if (_currAnim != null && _currAnimPriority > newAnimation.Priority && !overridePriority || _currAnim == newAnimation && !resetSameAnim)
            return;
        _currAnim = newAnimation;
        _currAnimPriority = newAnimation.Priority;
        if(_currFrameIndex > newAnimation.Frames.Length-1)
            _currFrameIndex = newAnimation.Frames.Length-1;
    }
    public void EndAnimation(CustomAnimation endingAnimation) => EndAnimation(endingAnimation.AnimationName);
    public void EndAnimation(string animationName)
    {
        if (_currAnim.AnimationName != animationName)
            return;
        _currAnimPriority = 0;
    }
}
