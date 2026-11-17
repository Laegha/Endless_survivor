using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LineXConfig
{
    [SerializeField] LineXData _lineData;
    //ADD SOMETHING THAT MAKES THE CURVE A FRAGMENT OF THE FULL LINE OR THE FULL LINE ITSELF (like if the curve repeats until the line is done or if the curve is the full line)
    [SerializeField] AnimationCurve _lineCurve;
    [SerializeField] float _curveMultiplier = 1;
    [Range(0,1)][SerializeField] float _distBetweenVertices;
    Vector2 _initialPos;
    Vector2 _horizontalDir;
    float _totalDist;
    Func<bool> _abortCondition;

    public LineXData LineXData {  get { return _lineData; } }
    public AnimationCurve LineCurve { get { return _lineCurve; } }
    public float CurveMultiplier { get { return _curveMultiplier; } }
    public float DistBetweenVertices {  get { return _distBetweenVertices; } }
    public float TotalDist {  get { return _totalDist; } }
    public Vector2 InitialPos { get { return _initialPos; } }
    public Vector2 HorizontalDir { get { return _horizontalDir; } }
    public Func<bool> AbortCondition { get { return _abortCondition; } }

    public LineXConfig(LineXData lineData, AnimationCurve lineCurve, float curveMultiplier, float distBetweenVertices, Vector2 initialPos, Vector2 horizontalDir, float totalDist, Func<bool> abortCondition)
    {
        _lineData = lineData;
        _lineCurve = lineCurve;
        _curveMultiplier = curveMultiplier;
        _distBetweenVertices = distBetweenVertices;
        _initialPos = initialPos;
        _horizontalDir = horizontalDir;
        _totalDist = totalDist;
        _abortCondition = abortCondition;
    }
    public LineXConfig(LineXConfig original, Vector2 initialPos, Vector2 horizontalDir, float totalDist, Func<bool> abortCondition)
    {
        _lineData = original._lineData;
        _lineCurve = original._lineCurve;
        _curveMultiplier = original._curveMultiplier;
        _distBetweenVertices = original._distBetweenVertices;
        _initialPos = initialPos;
        _horizontalDir = horizontalDir;
        _totalDist = totalDist;
        _abortCondition = abortCondition;
    }
}
