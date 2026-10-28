using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DinamycJoystickHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    int _timesPressed;
    [SerializeField] bl_Joystick _joystick;
    GameObject _joystickGo;
    RectTransform _joystickTr;

    public Vector2 InputDir { get
        {
            if(_timesPressed <= 0)
                return Vector2.zero;
            return new(_joystick.Horizontal, _joystick.Vertical);
        } }
    private void Start()
    {
        _joystickGo = _joystick.gameObject;
        _joystickTr = _joystick.GetComponent<RectTransform>();
    }
    public void OnPointerDown(PointerEventData data)
    {
        _timesPressed++;
        if (_timesPressed > 1)
            return;
        _joystickGo.SetActive(true);
        _joystickTr.position = data.position;
        _joystick.UpdateCenterInfo();
        _joystick.OnPointerDown(data);
    }

    public void OnPointerUp(PointerEventData data)
    {
        _timesPressed--;
        if (_timesPressed > 0) 
            return;
        _joystickGo.SetActive(false);
    }
    public void OnDrag(PointerEventData data)
    {
        _joystick.OnDrag(data);
    }
}
