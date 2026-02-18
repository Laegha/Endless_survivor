using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSupportObjsOnFirstActiveEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] SupportObjectData[] _createdSupportObjs;
    [SerializeField] Vector2 _objsOffset;
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var createSupportObjOriginal = original as CreateSupportObjsOnFirstActiveEnemyBehaviour;
        _createdSupportObjs = createSupportObjOriginal._createdSupportObjs;
        _objsOffset = createSupportObjOriginal._objsOffset;
    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        //create obj
        foreach(var obj in _createdSupportObjs)
        {
            Utility.GenerateSupportObj(obj, (Vector2)EnemyControl.transform.position + _objsOffset, Quaternion.identity);
        }
        KillBehaviour();
    }
}
