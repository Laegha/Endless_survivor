using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineXInfo
{
    LineXData _lineData; 
    AnimationCurve _lineCurve;
    float _curveMultiplier = 1;
    float _lineDissapearSpeed;
    float _distBetweenVertices;
    Vector2 _initialPos;
    Vector2 _horizontalDir;
    Vector2 _verticalDir;
    float _totalDist;
    float _lapsedDistance;
    List<(float, Vector2)> _lineVertices = new();
    LineRenderer _line;
    Func<bool> _abortCondition;

    public LineRenderer Line { get { return _line; } }


    public LineXInfo(LineXData lineData, AnimationCurve lineCurve, float curveMultiplier, float lineDissapearSpeed, float distBetweenVertices, Vector2 initialPos, Vector2 horizontalDir, float totalDist, Func<bool> abortCondition)
    {
        _lineData = lineData;
        _lineCurve = lineCurve;
        _curveMultiplier = curveMultiplier;
        _lineDissapearSpeed = lineDissapearSpeed;
        _distBetweenVertices = distBetweenVertices;
        _initialPos = initialPos;
        _horizontalDir = horizontalDir;
        _verticalDir = Utility.GetPerpendicularVector(_horizontalDir);
        if (_horizontalDir.x < 0)
            _verticalDir *= -1;
        _totalDist = totalDist;
        _abortCondition = abortCondition;
    }

    public void DrawLine(LineRenderer lineRenderer)
    {
        _line = lineRenderer;
        lineRenderer.material = _lineData.LineMaterial;
        lineRenderer.widthMultiplier = _lineData.LineWidthMultiplier;
        lineRenderer.widthCurve = _lineData.LineWidth;
        int totalVertices = (int)Mathf.Ceil(1 / _distBetweenVertices) + 1;
        lineRenderer.positionCount = totalVertices;
        for (int i = 0; i < totalVertices; i++)
        {
            Vector2 xDisplacement = _horizontalDir * i * _distBetweenVertices * _totalDist;
            Vector2 yDisplacement = _verticalDir * _lineCurve.Evaluate(i * _distBetweenVertices) * _curveMultiplier;
            Vector2 vertex = _initialPos + xDisplacement + yDisplacement;

            _lineVertices.Add(new(i * _distBetweenVertices, vertex));
            lineRenderer.SetPosition(i, vertex);
        }
    }

    public bool ProgressLine()
    {
        if (_abortCondition != null && _abortCondition.Invoke())
            return true;
        Vector2 xMovement = _horizontalDir * _lapsedDistance;
        Vector2 yMovement = _verticalDir * _lineCurve.Evaluate(Mathf.Clamp01(_lapsedDistance / _totalDist)) * _curveMultiplier;
        Vector2 newPos = _initialPos + xMovement + yMovement;

        _lapsedDistance += _lineDissapearSpeed * Time.deltaTime;
        if (_lineVertices[1].Item1 < _lapsedDistance / _totalDist)
        {
            RemoveFirstVertex();
        }
        if (_lineVertices.Count == 1)
            return true;
        _line.SetPosition(0, newPos);
        return false;
    }
    void RemoveFirstVertex()
    {
        _line.positionCount--;
        _lineVertices.RemoveAt(0);
        for (int i = 0; i < _lineVertices.Count; i++)
        {
            if (i > _line.positionCount)
            {
                Debug.LogError("Generated LineX has more vertex in the script than there are in the vertex dictionary");
                break;
            }
            _line.SetPosition(i, _lineVertices[i].Item2);
        }
    }
}
