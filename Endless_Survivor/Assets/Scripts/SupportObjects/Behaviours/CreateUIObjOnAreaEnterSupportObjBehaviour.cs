using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateUIObjOnAreaEnterSupportObjBehaviour : UseAreaAroundSupportObjBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] GameObject _objPrefab;
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var createUIObjOriginal = original as CreateUIObjOnAreaEnterSupportObjBehaviour;
        _objPrefab = createUIObjOriginal._objPrefab;
        OnObjEnterArea += CreateObj;
    }

    void CreateObj(GameObject placeholder)
    {
        var canvas = GameUIManager.uiManager.transform.root;
        GameObject.Instantiate(_objPrefab, canvas);
    }
}
