using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CheckAreaAroundSupportObjBehaviour : SupportObjectBehaviour
{
    public new static bool isUsable => true;
    [SerializeField] string _areaName = "AreaIdentifier";
    [SerializeField] Vector2 _areaSize;
    [SerializeField] Vector2 _areaOffset;
    [SerializeField] Sprite _areaGfx;
    [SerializeField] float _areaGfxAlpha;
    List<GameObject> _objsInArea = new();

    Action<GameObject> _onObjEnterArea;
    Action<GameObject> _onObjUpdateArea;
    Action<GameObject> _onObjExitArea;
    public string AreaName { get { return _areaName; } }
    public Action<GameObject> OnObjEnterArea { get { return _onObjEnterArea; } set { _onObjEnterArea = value; } }
    public Action<GameObject> OnObjUpdateArea { get { return _onObjUpdateArea; } set { _onObjUpdateArea = value; } }
    public Action<GameObject> OnObjExitArea { get { return _onObjExitArea; } set { _onObjExitArea = value; } }
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var checkAreaOriginal = original as CheckAreaAroundSupportObjBehaviour;
        _areaName = checkAreaOriginal._areaName;
        _areaSize = checkAreaOriginal._areaSize;
        _areaOffset = checkAreaOriginal._areaOffset;
        _areaGfx = checkAreaOriginal._areaGfx;
        _areaGfxAlpha = checkAreaOriginal._areaGfxAlpha;
        OnStart += CreateAreaGfx;
        OnUpdate += CheckArea;
    }
    void CreateAreaGfx()
    {
        var areaGameObj = new GameObject("AreaGFX");
        var areaRenderer = areaGameObj.AddComponent<SpriteRenderer>();
        areaRenderer.sprite = _areaGfx;
        areaRenderer.sortingOrder = 10000;
        areaRenderer.color = new Color(areaRenderer.color.r, areaRenderer.color.g, areaRenderer.color.b, _areaGfxAlpha / 255);
        Debug.Log(_areaGfx.bounds.extents);
        areaGameObj.transform.localScale = _areaSize / (_areaGfx.bounds.extents);
        areaGameObj.transform.SetParent(ObjControl.transform);
        areaGameObj.transform.localPosition = Vector2.zero;
        ObjControl.Renderers.Add(areaRenderer);
    }
    void CheckArea()
    {
        var colsInArea = Physics2D.OverlapBoxAll((Vector2)ObjControl.transform.position + _areaOffset, _areaSize, 0);
        foreach(var col in colsInArea)
        {
            if(!_objsInArea.Contains(col.gameObject))
            {
                _objsInArea.Add(col.gameObject);
                _onObjEnterArea?.Invoke(col.gameObject);
            }
        }
        var objsInAreaCopy = new List<GameObject>(_objsInArea);
        foreach(var obj in objsInAreaCopy)
        {
            if(obj == null)
            {
                _objsInArea.Remove(obj);
                continue;
            }
            if(!colsInArea.Contains(obj.GetComponent<Collider2D>()))
            {
                _onObjExitArea?.Invoke(obj);
                _objsInArea.Remove(obj);
                continue;
            }
            _onObjUpdateArea?.Invoke(obj);
        }
    }
}
