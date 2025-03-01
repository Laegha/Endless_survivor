using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAnimator : MonoBehaviour
{
    CustomAnimation _currAnim;
    int _currFrameIndex;
    CharacterData _characterData;
    [SerializeField] SpriteRenderer _spriteRenderer;

    float _animTimer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_animTimer <= 0)
            NextFrame();
    }

    public void NextFrame()
    {
        _spriteRenderer.sprite = _currAnim.Frames[_currFrameIndex];
        _currFrameIndex++;
    }

    public void ChangeAnim(string anim)
    {
        _currAnim = _characterData.Animations[anim];
        _animTimer = 0;
    }
}
