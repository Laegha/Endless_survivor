using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRenderOffsetSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] int _renderOffset;

    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var setRenderOriginal = original as SetRenderOffsetSupportObjBehaviour;
        _renderOffset = setRenderOriginal._renderOffset;
        OnStart += SetOffset;
    }

    void SetOffset()
    {
        ObjControl.RendererSorter.Offset = _renderOffset;
    }
}
