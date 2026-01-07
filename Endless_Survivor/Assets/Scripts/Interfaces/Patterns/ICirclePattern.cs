using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ICirclePattern : IPattern
{
    [SerializeField] float _radius = 2f;
    [SerializeField] float _startAngle;
    [SerializeField] bool _fitCircle = true;
    [SerializeField] float _angleStep = 30f;

    public IEnumerable<Vector2> GetPositions(Vector2 origin, int count)
    {
        float step = _fitCircle ? 360f / count : _angleStep;

        for (int i = 0; i < count; i++)
        {
            float angle = _startAngle + step * i;
            yield return origin + Utility.GetPointInCircle(_radius, angle);
        }
    }
}
