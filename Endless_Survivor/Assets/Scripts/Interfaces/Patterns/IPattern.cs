using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPattern
{
    IEnumerable<Vector2> GetPositions(
        Vector2 origin,
        int count
    );
}
