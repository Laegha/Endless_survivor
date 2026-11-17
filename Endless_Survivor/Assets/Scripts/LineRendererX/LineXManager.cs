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
    List<(LineXInfo, Func<bool>)> _standbyLines = new();

    private void Awake()
    {
        instance = this;
    }

    public LineXInfo DrawLine(LineXConfig lineConfig)
    {
        if(lineConfig.LineXData == null)
            return null;
        LineXInfo lineInfo = new(lineConfig.LineXData, lineConfig.LineCurve, lineConfig.CurveMultiplier, lineConfig.DistBetweenVertices, lineConfig.InitialPos, lineConfig.HorizontalDir, lineConfig.TotalDist, lineConfig.AbortCondition);
        
        lineInfo.DrawLine();

        _standbyLines.Add((lineInfo, lineConfig.AbortCondition));
        return lineInfo;
    }
    private void Update()
    {
        List<(LineXInfo, Func<bool>)> standbyLinesCopy = new(_standbyLines);
        foreach (var lineCondition in standbyLinesCopy)
        {
            if (lineCondition.Item2 != null && lineCondition.Item2())
            {
                ObjectDestroyingManager.odm.DestroyObj(lineCondition.Item1.LineTr.transform.root.gameObject);
                _standbyLines.RemoveAll(x => x.Item1 == lineCondition.Item1);
            }
        }
    }
}
