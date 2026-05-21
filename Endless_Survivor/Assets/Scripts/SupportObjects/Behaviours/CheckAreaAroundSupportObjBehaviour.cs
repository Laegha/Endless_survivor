using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CheckAreaAroundSupportObjBehaviour : SupportObjectBehaviour
{
    public new static int maxStacks => -1;
    [SerializeField] string _areaName = "AreaIdentifier";
    [SerializeField] LayerMask _collidingLayers;
    [SerializeField] Vector2 _areaSize;
    [SerializeField] Vector2 _areaOffset;
    [SerializeField] CustomAnimation _areaGfx;
    [SerializeField] int _areaGfxSortingOffset; 
    [Range(0,255)][SerializeField] float _areaGfxAlpha;
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
        _collidingLayers = checkAreaOriginal._collidingLayers;
        _areaSize = checkAreaOriginal._areaSize;
        _areaOffset = checkAreaOriginal._areaOffset;
        _areaGfx = new(null, checkAreaOriginal._areaGfx);
        _areaGfxSortingOffset = checkAreaOriginal._areaGfxSortingOffset;
        _areaGfxAlpha = checkAreaOriginal._areaGfxAlpha;
        if(_areaGfx != null)
            OnStart += CreateAreaGfx;
        OnUpdate += CheckArea;
    }
    void CreateAreaGfx()
    {
        //make the area an animated obj and instead of changing sorting order change offset on the renderer sorter
        AnimatedObjConfig animatedObjConfig = new(_areaGfx, _areaOffset, Quaternion.identity, -1, ObjControl.transform, true, true);
        var gfxAnimator = AnimatedObjsManager.aom.SpawnAnimatedObj(animatedObjConfig);
        gfxAnimator.transform.root.GetComponentInChildren<RendererSortingByY>().Offset = _areaGfxSortingOffset;
        var areaRenderer = gfxAnimator.Renderer;
        areaRenderer.color = new Color(areaRenderer.color.r, areaRenderer.color.g, areaRenderer.color.b, _areaGfxAlpha / 255);
        ObjControl.Renderers.Add(areaRenderer);
    }
    void CheckArea()
    {
        var colsInArea = Physics2D.OverlapBoxAll((Vector2)ObjControl.transform.position + _areaOffset, _areaSize, 0, _collidingLayers);
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
