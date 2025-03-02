using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAnimator : MonoBehaviour
{
    CustomAnimation _currAnim;
    int _currFrameIndex;
    [SerializeField] SpriteRenderer _spriteRenderer;
    float _animTimer;

    public CustomAnimation CurrAnim { get {  return _currAnim; } set { _currAnim = value; } }
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
            _currFrameIndex = 0;
    }

    public virtual void ChangeAnim(string anim)
    {
        _animTimer = 0;
    }
}
