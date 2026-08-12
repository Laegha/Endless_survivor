using UnityEngine;

public class ObjectRowElement
{
    Transform _elementTr;
    Vector2 _prevPos;
    public Transform ElementTr {  get { return _elementTr; } }
    public Vector2 PrevPos { get { return _prevPos; } set { _prevPos = value; } }
    public Vector2 MovingDirection => ((Vector2)_elementTr.position - _prevPos).normalized;

    public ObjectRowElement(Transform elementTr, Vector2 prevPos)
    {
        _elementTr = elementTr;
        _prevPos = prevPos;
    }
}