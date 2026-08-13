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
    [Range(0,1)][SerializeField] float _distBetweenVertices;
    bool _dissapearOnStart;
    Vector2 _initialPos;
    Vector2 _horizontalDir;
    float _totalDist;
    Func<bool> _abortCondition;
    Transform _followingObj;

    public LineXData LineXData {  get { return _lineData; } }
    public AnimationCurve LineCurve { get { return _lineCurve; } }
    public float CurveMultiplier { get { return _curveMultiplier; } }
    public float LineDissapearSpeed { get { return _lineDissapearSpeed; } }
    public float DistBetweenVertices {  get { return _distBetweenVertices; } }
    public float TotalDist {  get { return _totalDist; } }
    public bool DissapearOnStart { get { return _dissapearOnStart; } }
    public Vector2 InitialPos { get { return _initialPos; } }
    public Vector2 HorizontalDir { get { return _horizontalDir; } }
    public Func<bool> AbortCondition { get { return _abortCondition; } }
    public Transform FollowingObj { get { return _followingObj; } }

    public LineXConfig(LineXData lineData, AnimationCurve lineCurve, float curveMultiplier, float lineDissapearSpeed, float distBetweenVertices, Vector2 initialPos, Vector2 horizontalDir, float totalDist, bool dissapearOnStart, Func<bool> abortCondition, Transform followingObj)
    {
        _lineData = lineData;
        _lineCurve = lineCurve;
        _curveMultiplier = curveMultiplier;
        _lineDissapearSpeed = lineDissapearSpeed;
        _distBetweenVertices = distBetweenVertices;
        _initialPos = initialPos;
        _horizontalDir = horizontalDir;
        _totalDist = totalDist;
        _dissapearOnStart = dissapearOnStart;
        _abortCondition = abortCondition;
        _followingObj = followingObj;
    }
    public LineXConfig(LineXConfig original, Vector2 initialPos, Vector2 horizontalDir, float totalDist, bool dissapearOnStart, Func<bool> abortCondition, Transform followingObj)
    {
        _lineData = original._lineData;
        _lineCurve = original._lineCurve;
        _curveMultiplier = original._curveMultiplier;
        _lineDissapearSpeed = original._lineDissapearSpeed;
        _distBetweenVertices = original._distBetweenVertices;
        _initialPos = initialPos;
        _horizontalDir = horizontalDir;
        _totalDist = totalDist;
        _dissapearOnStart = dissapearOnStart;
        _abortCondition = abortCondition;
        _followingObj = followingObj;
    }
}
