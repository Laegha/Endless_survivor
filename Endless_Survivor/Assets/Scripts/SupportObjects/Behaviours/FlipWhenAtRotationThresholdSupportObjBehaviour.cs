using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipWhenAtRotationThresholdSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] List<AngleFlipThreshold> _flipThresholds = new List<AngleFlipThreshold>();
    [SerializeField] int _childRotationCheckIndex;
    [SerializeField] bool _overrideRotation = true;
    [SerializeField] bool _affectChildren = true;

    Transform _rotationCheckTr;
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var flipOriginal = original as FlipWhenAtRotationThresholdSupportObjBehaviour;
        _flipThresholds = flipOriginal._flipThresholds;
        _childRotationCheckIndex = flipOriginal._childRotationCheckIndex;
        _overrideRotation = flipOriginal._overrideRotation;
        _affectChildren = flipOriginal._affectChildren;

        OnStart += GetTr;
        OnLateUpdate += CheckRotation;
    }
    void GetTr()
    {
        Debug.Log("LENG " + ObjControl.GetComponentsInChildren<Transform>().Length);
        foreach (var tr in ObjControl.GetComponentsInChildren<Transform>())
            Debug.Log("CHILD " + tr.name);
        _rotationCheckTr = ObjControl.GetComponentsInChildren<Transform>()[_childRotationCheckIndex];
    }
    void CheckRotation()
    {
        Debug.Log(_rotationCheckTr.rotation.z);
        var currThreshold = _flipThresholds.Find(threshold => threshold.IsInThreshold(_rotationCheckTr.rotation.eulerAngles.z));
        if (currThreshold == null)
            return;
        foreach(var renderer in ObjControl.Renderers)
        {
            renderer.flipX = currThreshold.isFlipped;
        }
        if (!_overrideRotation)
            return;
        ObjControl.transform.rotation = Quaternion.identity;
        if (!_affectChildren)
            return;
        var children = ObjControl.GetComponentsInChildren<Transform>();
        foreach(var child in children)
            child.transform.rotation = Quaternion.identity;
    }
}
[Serializable]
public class AngleFlipThreshold
{
    public float minAngle;
    public float maxAngle;
    public bool isFlipped;

    public bool IsInThreshold(float angle)
    {
        return angle >= minAngle && angle < maxAngle;
    }
}