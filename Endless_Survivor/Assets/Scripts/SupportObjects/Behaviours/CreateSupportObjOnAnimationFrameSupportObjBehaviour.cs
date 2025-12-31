using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CreateSupportObjOnAnimationFrameSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] string _animationName;
    [SerializeField] int _createFrame;
    [SerializeField] SupportObjectData _createdSupportObj;
    [SerializeField] Vector2 _positionOffset;
    [SerializeField] bool _loop;
    [SerializeField] bool _destroyOriginal;

    bool _created;

    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var createSupportObjOriginal = original as CreateSupportObjOnAnimationFrameSupportObjBehaviour;
        _animationName = createSupportObjOriginal._animationName;
        _createFrame = createSupportObjOriginal._createFrame;
        _createdSupportObj = createSupportObjOriginal._createdSupportObj;
        _positionOffset = createSupportObjOriginal._positionOffset;
        _loop = createSupportObjOriginal._loop;
        _destroyOriginal = createSupportObjOriginal._destroyOriginal;
        OnStart += AddAnimEvent;
    }
    void AddAnimEvent()
    {
        var anim = ObjControl.Animator.Animations.Find(x => x.AnimationName == _animationName);
        anim.Events.Add(new(null, _createFrame, CreateObj));
    }
    void CreateObj()
    {
        if (!_loop && _created)
            return;
        _created = true;
        Utility.GenerateSupportObj(_createdSupportObj, (Vector2)ObjControl.transform.position + _positionOffset, Quaternion.identity);
        if (_destroyOriginal)
        {
            GameObject.Destroy(ObjControl.gameObject);
            return;
        }
    }
}
