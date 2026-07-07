using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using HorizontalAlignment = CustomFlags.HorizontalAlignment;
using VerticalAlignment = CustomFlags.VerticalAlignment;

[Serializable]
public class ILinePattern : IPattern
{
    [SerializeField] Vector2 _spacing;
    [SerializeField] HorizontalAlignment _hAlignment;
    [SerializeField] VerticalAlignment _vAlignment;
    
    public IEnumerable<Vector2> GetPositions(Vector2 origin, int count)
    {
        Vector2 newSpacing = new(_spacing.x * (_hAlignment == HorizontalAlignment.Right ? -1 : 1), _spacing.y * (_vAlignment == VerticalAlignment.Top ? -1 : 1));
        Vector2 startPos = origin - new Vector2(_hAlignment == HorizontalAlignment.Center ? newSpacing.x / 2 : 0, _vAlignment == VerticalAlignment.Center ? newSpacing.y / 2 : 0);
        
        for (int i = 0; i < count; i++)
        {
            yield return startPos + newSpacing * i;
        }
    }
}
