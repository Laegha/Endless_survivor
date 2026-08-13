using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LineXManager : MonoBehaviour
{
    static LineXManager instance;
    public static LineXManager lm {  get { return instance; } }
    [SerializeField] GameObject _linePrefab;
    List<LineXInfo> _dissapearingLines = new();
    List<(LineXInfo, Func<bool>)> _standbyLines = new();

    private void Awake()
    {
        instance = this;
    }

    public LineXInfo DrawLine(LineXConfig lineConfig)
    {
        LineXInfo lineInfo = new(lineConfig.LineXData, lineConfig.LineCurve, lineConfig.CurveMultiplier, lineConfig.LineDissapearSpeed, lineConfig.DistBetweenVertices, lineConfig.InitialPos, lineConfig.HorizontalDir, lineConfig.TotalDist, lineConfig.AbortCondition, lineConfig.FollowingObj);
        var createdLineObj = GameObject.Instantiate(_linePrefab);
        createdLineObj.GetComponent<LineSortingOrderByY>().Offset = lineConfig.LineXData.LineRenderOffset;
        var createdLine = createdLineObj.GetComponentInChildren<LineRenderer>();
        lineInfo.DrawLine(createdLine);
        if (lineConfig.DissapearOnStart)
            StartLineDissapearing(lineInfo);
        else
            _standbyLines.Add((lineInfo, lineConfig.AbortCondition));
        return lineInfo;
    }
    public void StartLineDissapearing(LineXInfo startingLine)
    {
        _dissapearingLines.Add(startingLine);
        _standbyLines.RemoveAll(x => x.Item1 == startingLine);
    }

    private void Update()
    {
        List<LineXInfo> dissapearingLinesCopy = new(_dissapearingLines);
        foreach (var line in dissapearingLinesCopy)
        {
            if (line.ProgressLine())
            {
                ObjectDestroyingManager.odm.DestroyObj(line.Line.transform.root.gameObject);
                _dissapearingLines.Remove(line);
            }

        }
        List<(LineXInfo, Func<bool>)> standbyLinesCopy = new(_standbyLines);
        foreach (var lineCondition in standbyLinesCopy)
        {
            if (lineCondition.Item2 != null && lineCondition.Item2())
            {
                ObjectDestroyingManager.odm.DestroyObj(lineCondition.Item1.Line.transform.root.gameObject);
                _standbyLines.RemoveAll(x => x.Item1 == lineCondition.Item1);
            }
        }

    }
}
