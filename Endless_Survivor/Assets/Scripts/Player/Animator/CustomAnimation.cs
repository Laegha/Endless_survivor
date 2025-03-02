using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomAnimation
{
    [SerializeField] string _animationName;
    [SerializeField]Sprite[] _frames;
    [SerializeField]float _framesPerSecond;

    public string AnimationName {  get { return _animationName; } }
    public Sprite[] Frames {  get { return _frames; } }
    public float FramesPerSecond {  get { return _framesPerSecond; } }
}
