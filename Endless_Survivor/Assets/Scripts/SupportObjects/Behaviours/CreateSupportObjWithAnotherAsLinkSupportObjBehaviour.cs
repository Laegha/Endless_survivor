using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSupportObjWithAnotherAsLinkSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] SupportObjectData _linkSupportObj;
    [SerializeField] SupportObjectData _finalSupportObj;
    SupportObjectControl _createdObjControl;
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var createSupportObjOriginal = original as CreateSupportObjWithAnotherAsLinkSupportObjBehaviour;
        _linkSupportObj = createSupportObjOriginal._linkSupportObj;
        _finalSupportObj = createSupportObjOriginal._finalSupportObj;
        OnStart += CreateObj;
        OnUpdate += CheckForObjDestroyed;
    }

    void CreateObj()
    {
        _createdObjControl = Utility.GenerateSupportObj(_linkSupportObj, ObjControl.transform.position, Quaternion.identity);
        
    }

    void CheckForObjDestroyed()
    {
        if (_createdObjControl != null)
            return;
        Utility.GenerateSupportObj(_linkSupportObj, ObjControl.transform.position, Quaternion.identity);
        GameObject.Destroy(ObjControl.gameObject);
    }
}
