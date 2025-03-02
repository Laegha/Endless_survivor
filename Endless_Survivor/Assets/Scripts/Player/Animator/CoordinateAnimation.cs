using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateAnimation : CustomAnimation
{
    [SerializeField] Vector2 _coordinates;
    public Vector2 Coordinates {  get { return _coordinates; } }
}
