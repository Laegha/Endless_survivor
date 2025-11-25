using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchControls : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] UILine _linePrefab;
    [SerializeField] RectTransform _linesHolder;
    [SerializeField] float _lineDestroyAfterReleaseDelay = .5f;
    UILine _currLine;
    Vector2 _draggingDirection;
    Vector2 _previousTouchPos;
    readonly float _dragDeadZone = 30;
    readonly float _dragDistanceDeadZone = 100;
    public Vector2 DraggingDirection {  get { return _draggingDirection; } }
    public void OnPointerDown(PointerEventData data) 
    {
        _currLine = Instantiate(_linePrefab, _linesHolder);
        _currLine.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
    public void OnDrag(PointerEventData data) 
    {
        Vector2 currTouchPos = Utility.TouchPosition();
        Vector2 dragDirection = currTouchPos - _previousTouchPos;
        if (dragDirection.magnitude <= _dragDeadZone)
            return;
        Vector2 touchPosInCanvas = Utility.ScreenToCanvas(currTouchPos - Utility.ScreenSize/2);
        Vector2 dragDistance = touchPosInCanvas - GetComponent<RectTransform>().anchoredPosition;
        print(dragDistance.magnitude);

        _currLine.AddVertex(dragDistance.magnitude > _dragDistanceDeadZone ? dragDistance.normalized * _dragDistanceDeadZone : touchPosInCanvas);
        _draggingDirection = dragDirection;
        _previousTouchPos = currTouchPos;
    }
    public void OnPointerUp(PointerEventData data)
    {
        _draggingDirection = Vector2.zero;
        UILineDissapearAnimator uILineDissapearAnimator = _currLine.GetComponent<UILineDissapearAnimator>();
        uILineDissapearAnimator.OnOneVertexLeft += () => Destroy(uILineDissapearAnimator.gameObject, _lineDestroyAfterReleaseDelay);
    }
}
