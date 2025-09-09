using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CustomLineAnimatedTile
{
    SpriteRenderer _renderer;
    [SerializeField]float _framesPerSecond;
    [SerializeField] Sprite[] _frames;

    int _currFrame = -1;
    float _frameTimer = 0;

    public float FramesPerSecond { get { return _framesPerSecond; } }
    public Sprite[] Frames { get { return _frames; } }

    public CustomLineAnimatedTile(SpriteRenderer renderer, float framesPerSecond, Sprite[] frames)
    {
        _renderer = renderer;
        _framesPerSecond = framesPerSecond;
        _frames = frames;
    }
    public void Update()
    {
        _frameTimer -= Time.deltaTime;
        if(_frameTimer <= 0)
        {
            NextFrame();
        }
    }

    void NextFrame()
    {
        _frameTimer = 1 / _framesPerSecond;
        _currFrame++;
        if (_currFrame >= _frames.Length)
            _currFrame = 0;

        _renderer.sprite = _frames[_currFrame];
    }
}