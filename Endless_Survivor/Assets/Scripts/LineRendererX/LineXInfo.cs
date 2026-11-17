using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;

public class LineXInfo
{
    LineXData _lineData;
    AnimationCurve _lineCurve;
    float _curveMultiplier = 1;
    float _distBetweenVertices;
    Vector2 _initialPos;
    Vector2 _horizontalDir;
    Vector2 _verticalDir;
    float _totalDist;
    List<Vector2> _lineVertices = new();
    Func<bool> _abortCondition;
    GameObject _startObj;
    GameObject _endObj;

    Transform _lineTr;

    List<SpriteRenderer> _tiles = new();
    List<CustomLineModifierBase> _activeLineModifiers = new();

    public Transform LineTr {  get { return _lineTr; } }
    public List<SpriteRenderer> LineTiles { get { return _tiles; } }

    public LineXInfo(LineXData lineData, AnimationCurve lineCurve, float curveMultiplier, float distBetweenVertices, Vector2 initialPos, Vector2 horizontalDir, float totalDist, Func<bool> abortCondition)
    {
        _lineData = lineData;
        _lineCurve = lineCurve;
        _curveMultiplier = curveMultiplier;
        _distBetweenVertices = distBetweenVertices;
        _initialPos = initialPos;
        _horizontalDir = horizontalDir.normalized;
        _verticalDir = Utility.GetPerpendicularVector(_horizontalDir).normalized;
        if (_horizontalDir.x < 0)
            _verticalDir *= -1;
        _totalDist = totalDist;
        _abortCondition = abortCondition;
    }

    public void DrawLine()
    {
        GameObject lineObj = new(_lineData.name + "_CustomLine");
        lineObj.transform.position = _initialPos;
        _lineTr = lineObj.transform;
        int totalVertices = (int)Mathf.Ceil(1 / _distBetweenVertices) + 1;

        for (int i = 0; i < totalVertices - 1; i++)
        {
            Vector2 currVertex = GetVertexPosById(i);
            Vector2 nextVertex = GetVertexPosById(i + 1);

            GenerateLineSegment(currVertex, nextVertex, _lineData.TileInfo);
        }
        float startObjRotation = 0;
        float endObjRotation = 0;
        if (_lineVertices.Count >= 2)
        {
            Vector2 startDir = _lineVertices[0] - _lineVertices[1];
            startDir = startDir.normalized;
            startObjRotation = Utility.GetAngleFromPointInCircle(startDir, false);

            Vector2 endDir = _lineVertices[_lineVertices.Count - 1] - _lineVertices[_lineVertices.Count - 2];
            endDir = endDir.normalized;
            endObjRotation = Utility.GetAngleFromPointInCircle(endDir, false);
        }
        if (_lineData.LineStartPointAnimation != null && _lineData.LineStartPointAnimation.Frames.Length > 0)
        {
            _startObj = AnimatedObjsManager.aom.SpawnAnimatedObj(new(_lineData.LineStartPointAnimation, _lineVertices[0], Quaternion.Euler(0, 0, startObjRotation), -1, null, false, false, _lineData.LineRenderOffset)).gameObject;
            
        }

        if (_lineData.LineEndPointAnimation != null && _lineData.LineEndPointAnimation.Frames.Length > 0)
        {
            _endObj = AnimatedObjsManager.aom.SpawnAnimatedObj(new(_lineData.LineEndPointAnimation, _lineVertices[_lineVertices.Count - 1], Quaternion.Euler(0, 0, endObjRotation), -1, null, false, false, _lineData.LineRenderOffset)).gameObject;

        }

        //Start modifiers
        foreach (var lineModifier in _lineData.LineModifiers)
        {
            AddLineModifier(lineModifier);
        }
    }
    Vector2 GetVertexPosById(int i)
    {
        Vector2 xDisplacement = _horizontalDir * i * _distBetweenVertices * _totalDist;
        Vector2 yDisplacement = _verticalDir * _lineCurve.Evaluate(i * _distBetweenVertices) * _curveMultiplier;
        Vector2 vertex = _initialPos + xDisplacement + yDisplacement;

        return vertex;
    }
    public void GenerateLineSegment(Vector2 startPosition, Vector2 endPosition, CustomLineTileInfo tileInfo)//sprite size in pixels
    {
        float tileSize = tileInfo.isVertical ? tileInfo.tileSize.y / 32 : tileInfo.tileSize.x / 32;

        Vector2 lineVector = endPosition - startPosition;
        float lineLength = lineVector.magnitude;
        int totalTiles = (int)(lineLength / tileSize);

        Vector2 lineDirection = lineVector.normalized;
        Vector2 firstTilePosition = startPosition + lineDirection * tileSize / 2;//we assume that the sprite's pivot point is centered
        float tileAngle = Mathf.Atan2(lineDirection.y, lineDirection.x);

        for (int i = 0; i < totalTiles; i++)
        {
            //tile GameObject generation and transform set
            var tileObj = new GameObject("LineTile " + i);
            tileObj.transform.position = firstTilePosition + lineDirection * tileSize * i;
            tileObj.transform.rotation = Quaternion.Euler(0, 0, tileAngle * Mathf.Rad2Deg - 90);
            tileObj.transform.parent = _lineTr;

            var tileRenderer = tileObj.AddComponent<SpriteRenderer>();
            tileRenderer.sprite = tileInfo.tileSprite;
            tileRenderer.material = _lineData.LineMaterial;
            var tileSpriteSorter = tileObj.AddComponent<RendererSortingByY>();
            tileSpriteSorter.AffectedRenderers = new SpriteRenderer[] { tileRenderer };
            //create tile object and add it to the list
            _tiles.Add(tileRenderer);
        }
    }
    void AddLineModifier(CustomLineModifierBase lineModifier)
    {
        CustomLineModifierBase activeModifier = (CustomLineModifierBase)Activator.CreateInstance(lineModifier.GetType());
        activeModifier.CopyValues(lineModifier, this);
        _activeLineModifiers.Add(activeModifier);

        activeModifier.OnStart?.Invoke();
    }
    void UpdateModifiers()
    {
        //var modifiersCopy = new List<CustomLineModifierBase>(_activeLineModifiers);
        foreach (var modifier in _activeLineModifiers)
        {
            modifier.OnUpdate?.Invoke();
        }
    }
    public T GetModifierByType<T>(Type modifierType) where T : CustomLineModifierBase
    {
        foreach (var modifier in _activeLineModifiers)
        {
            if (modifierType.GetType() == modifierType)
                return (T)modifier;
        }
        return null;
    }
}
