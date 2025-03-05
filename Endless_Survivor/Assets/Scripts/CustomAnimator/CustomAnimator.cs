using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomAnimator : MonoBehaviour
{
    List<CustomAnimation> _animations;
    CustomAnimation _currAnim;
    int _currFrameIndex;
    [SerializeField] SpriteRenderer _spriteRenderer;
    float _animTimer;

    public CustomAnimation CurrAnim { get {  return _currAnim; } set { _currAnim = value; } }
    public List<CustomAnimation> Animations{ get {  return _animations; } set { _animations = value; } }

    public virtual void AddAnimations(List<CustomAnimation> newAnimations) => newAnimations.ForEach(anim => _animations.Add(anim));

    void Update()
    {
        _animTimer -= Time.time;
        if(_animTimer <= 0)
        {
            NextFrame();
            _animTimer = 1/_currAnim.FramesPerSecond;
        }
    }

    public void NextFrame()
    {
        _spriteRenderer.sprite = _currAnim.Frames[_currFrameIndex];
        _currFrameIndex++;
        if(_currFrameIndex >= _currAnim.Frames.Length)
        {
            _currFrameIndex = 0;
            CurrAnim.OnAnimationEnd?.Invoke(this);
        }
    }

    public virtual void ChangeAnim(string animName)
    {
        _animTimer = 0;
        CurrAnim = _animations.Where(anim => anim.AnimationName == animName).ToList()[0];
    }

}
