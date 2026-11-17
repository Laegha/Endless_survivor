using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CustomLineModifierBase
{
    public static int maxStacks => 0;
    LineXInfo _lineInfo;
    Action _onStart;
    Action _onUpdate;

    public LineXInfo LineInfo { get { return _lineInfo; } }
    public Action OnStart { get { return _onStart; } set { _onStart = value; } }
    public Action OnUpdate { get { return _onUpdate; } set { _onUpdate = value; } }
    public virtual void CopyValues(CustomLineModifierBase original, LineXInfo lineXInfo)
    {
        _lineInfo = lineXInfo;
    }
}
