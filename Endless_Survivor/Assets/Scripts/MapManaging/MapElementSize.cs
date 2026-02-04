using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapElementSize
{
    [SerializeField] Vector2 _elementSquareSize;
    [SerializeField] List<Vector2> _elementOccupyingPositions;

    public List<Vector2> ElementOccupyingPositions {  get { return _elementOccupyingPositions; } }
}
