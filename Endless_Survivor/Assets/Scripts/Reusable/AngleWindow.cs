using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AngleWindow
{
    [Header("In degrees")]
    [SerializeField] float _minAngle;
    [SerializeField] float _maxAngle;

    public bool IsAngleInWindow(float angle)
    {
        float window = Mathf.DeltaAngle(_minAngle, _maxAngle);
        float angleDelta = Mathf.DeltaAngle(_minAngle, angle);
        return Mathf.Sign(window) == Mathf.Sign(angleDelta) && Mathf.Abs(angleDelta) <= Mathf.Abs(window);
    }
    float PutAngleInRange(float angle)
    {
        return (angle + 540) % 360 - 180;
    }
}
