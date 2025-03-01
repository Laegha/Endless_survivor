using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomAnimation
{
    [SerializeField]Sprite[] _frames;
    [SerializeField]float _framesPerSecond;

    public Sprite[] Frames {  get { return _frames; } }
    public float FramesPerSecond {  get { return _framesPerSecond; } }
}
