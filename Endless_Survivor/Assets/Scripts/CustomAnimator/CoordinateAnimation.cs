using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CoordinateAnimation : CustomAnimation
{
    [SerializeField] Vector2 _coordinates;
    public Vector2 Coordinates {  get { return _coordinates; } }

    public CoordinateAnimation(CoordinateAnimation original) : base(original)
    {

    }
}
