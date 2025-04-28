using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SFXInfo
{
    [SerializeField] AudioClip _sound;
    [SerializeField] float _volume = 1;
    [SerializeField] float _minPitchVariation = 0;
    [SerializeField] float _maxPitchVariation = 0;

    public AudioClip Sound { get { return _sound; } }
    public float Volume {  get { return _volume; } }
    public float MinPitchVariation {  get { return _minPitchVariation; } }
    public float MaxPitchVariation {  get { return _maxPitchVariation; } }
}
