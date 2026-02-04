using System;
using UnityEngine;

[Serializable]
public class MapElementInfo<T>
{
    [SerializeField] T _mapElement;
    [SerializeField] MapElementSize _elementSize;

    public T Element { get { return _mapElement; } }
    public MapElementSize MapElementSize { get { return _elementSize; } }

    public MapElementInfo(T element, MapElementSize elementSize)
    {
        _mapElement = element;
        _elementSize = elementSize;
    }
}
