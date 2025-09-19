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

    public CustomAnimation CurrAnim { get {  return _currAnim; } set { _currAnim = value; } }
    public int CurrFrameIndex { set { _currFrameIndex = value; } }
    public List<CustomAnimation> Animations{ get {  return _animations; } set { _animations = value; } }
    public float AnimTimer { get { return _animTimer; } set { _animTimer = value; } }

    public virtual void AddAnimations(List<CustomAnimation> newAnimations) 
    {
        foreach (var anim in newAnimations)
        {
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
            //CurrAnim.OnAnimationEnd?.Invoke(this);
        }
        _spriteRenderer.sprite = _currAnim.Frames[_currFrameIndex];
        _currAnim.Events.Find(animEvent => animEvent.frameIndex == _currFrameIndex)?.frameAction?.Invoke();//this doesn't work if the event is set for the frame 0, since the event is called after the frame changed
        _animTimer = 1 / _currAnim.FramesPerSecond;
        //Debug.Log("EVENTS FOR FRAME " + _currFrameIndex + " IN ANIMATION " + _currAnim.AnimationName + _currAnim.Events.Find(animEvent => animEvent.frameIndex == _currFrameIndex));
    }

    public virtual void ChangeAnim(string animName, bool overridePriority = false)
    {
        CustomAnimation newAnimation = _animations.Find(anim => anim.AnimationName == animName);
        if(newAnimation == null)
        {
            Debug.LogError("ERROR: Animation not found: " +  animName + " on animator " + gameObject.name);
            return;
        }
        if(_currAnim != null && _currAnim.Priority > newAnimation.Priority && !overridePriority || _currAnim == newAnimation)
            return;
        _currAnim = newAnimation;
        _currFrameIndex = -1;
        NextFrame();
    }

}
