using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CreateSupportObjAfterTimeSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] SupportObjectData _createdSupportObj;
    [SerializeField] Vector2 _positionOffset;
    [SerializeField] float _timeToGenerate;
    [SerializeField] bool _loop;
    [SerializeField] bool _destroyOriginal;
    float _timer;
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var createSupportObjOriginal = original as CreateSupportObjAfterTimeSupportObjBehaviour;
        _createdSupportObj = createSupportObjOriginal._createdSupportObj;
        _positionOffset = createSupportObjOriginal._positionOffset;
        _timeToGenerate = createSupportObjOriginal._timeToGenerate;
        _loop = createSupportObjOriginal._loop;
        _destroyOriginal = createSupportObjOriginal._destroyOriginal;
        _timer = _timeToGenerate;
        OnUpdate += ReduceTimer;
    }

    void ReduceTimer()
    {
        _timer -= Time.deltaTime;
        if (_timer > 0)
            return;
        Utility.GenerateSupportObj(_createdSupportObj, (Vector2)ObjControl.transform.position + _positionOffset, Quaternion.identity);
        if(_destroyOriginal)
        {
            GameObject.Destroy(ObjControl.gameObject);
            return;
        }
        if (_loop)
            _timer = _timeToGenerate;
        else
            OnUpdate -= ReduceTimer;
    }
}
