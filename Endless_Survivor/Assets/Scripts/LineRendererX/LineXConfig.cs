using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LineXConfig
{
    [SerializeField] LineXData _lineData;
    [SerializeField] AnimationCurve _lineCurve;
    [SerializeField] float _curveMultiplier = 1;
    [SerializeField] float _lineDissapearSpeed;
    [SerializeField] float _distBetweenVertices;
    [SerializeField] bool _dissapearOnStart;
    Vector2 _initialPos;
    Vector2 _horizontalDir;
    float _totalDist;

    public LineXData LineXData {  get { return _lineData; } }
    public AnimationCurve LineCurve { get { return _lineCurve; } }
    public float CurveMultiplier { get { return _curveMultiplier; } }
    public float LineDissapearSpeed { get { return _lineDissapearSpeed; } }
    public float DistBetweenVertices {  get { return _distBetweenVertices; } }
    public float TotalDist {  get { return _totalDist; } }
    public bool DissapearOnStart { get { return _dissapearOnStart; } }
    public Vector2 InitialPos { get { return _initialPos; } }
    public Vector2 HorizontalDir { get { return _horizontalDir; } }
    
    public LineXConfig(LineXData lineData, AnimationCurve lineCurve, float curveMultiplier, float lineDissapearSpeed, float distBetweenVertices, Vector2 initialPos, Vector2 horizontalDir, float totalDist, bool dissapearOnStart)
    {
        _lineData = lineData;
        _lineCurve = lineCurve;
        _curveMultiplier = curveMultiplier;
        _lineDissapearSpeed = lineDissapearSpeed;
        _distBetweenVertices = distBetweenVertices;
        _totalDist = totalDist;
        _initialPos = initialPos;
        _horizontalDir = horizontalDir;
        _dissapearOnStart = dissapearOnStart;
    }
}
