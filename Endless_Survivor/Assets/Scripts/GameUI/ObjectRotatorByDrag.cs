using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectRotatorByDrag : MonoBehaviour, IDragHandler
{ 
    [SerializeField]Transform _center;
    [SerializeField]Transform _rotatedObject;
    [SerializeField]float _rotationLimit;

    Action _onLimitReached;

    bool _isActive = false;
    bool _dragging = false;
    float _previousDraggingAngle;
    float _draggedDistance = 0;

    public Action OnLimitReached { get { return _onLimitReached; } set { _onLimitReached = value; } }
    public bool IsActive { set
        {
            _isActive = value;
            if (value)
            {
                _draggedDistance = 0;
                _previousDraggingAngle = 0;
            }
        } 
    }
    public void OnDrag(PointerEventData data)
    {
        if(!_isActive)
        {
            return;
        }
        Vector2 pointerDist = data.position - (Vector2)_center.position;

        var draggingAngle = Utility.GetAngleFromPointInCircle(pointerDist.normalized);
        if (!_dragging)
        {
            _dragging = true;
        }
        var dragDelta = draggingAngle - _previousDraggingAngle;
        var draggingAngleFixed = draggingAngle;//considers the possibility of being dragged a full circle
        if (dragDelta > 100)//if the difference is too high, we asume it did a full circle clockwise
            draggingAngleFixed -= 360;
        if (dragDelta < -100)//if the difference is too low, we asume it did a full circle counterclockwise
            draggingAngleFixed += 360;

        var dragDeltaFixed = draggingAngleFixed - _previousDraggingAngle;
        _rotatedObject.Rotate(Vector3.forward * -dragDelta);
        _draggedDistance += dragDeltaFixed;
        _previousDraggingAngle = draggingAngle;
        if (_draggedDistance >= _rotationLimit)
            _onLimitReached?.Invoke();
    }
}
