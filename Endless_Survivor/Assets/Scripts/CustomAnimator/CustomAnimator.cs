using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomAnimator : MonoBehaviour
{
    List<CustomAnimation> _animations = new List<CustomAnimation>();
    CustomAnimation _currAnim;
    int _currFrameIndex;
    [SerializeField] SpriteRenderer _spriteRenderer;
    float _animTimer = 0;

    public CustomAnimation CurrAnim { get {  return _currAnim; } set { _currAnim = value; } }
    public List<CustomAnimation> Animations{ get {  return _animations; } set { _animations = value; } }
    public float AnimTimer { get { return _animTimer; } set { _animTimer = value; } }

    public virtual void AddAnimations(List<CustomAnimation> newAnimations) => newAnimations.ForEach(anim => _animations.Add(anim));

    void Update()
    {
        if (_currAnim == null)
            return;
        _animTimer -= Time.deltaTime;
        if(_animTimer <= 0)
        {
            NextFrame();
            _animTimer = 1/_currAnim.FramesPerSecond;
        }
    }

    public void NextFrame()
    {
        _currFrameIndex++;
        if(_currFrameIndex >= _currAnim.Frames.Length)
        {
            _currFrameIndex = 0;
            CurrAnim.OnAnimationEnd?.Invoke(this);
        }
        _spriteRenderer.sprite = _currAnim.Frames[_currFrameIndex];
    }

    public virtual void ChangeAnim(string animName)
    {
        CustomAnimation newAnimation = _animations.Where(anim => anim.AnimationName == animName).ToList()[0];
        if(_currAnim.Priority > newAnimation.Priority)
            return;
        _currAnim = newAnimation;
        _animTimer = 0;
    }

}
