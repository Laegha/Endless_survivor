using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateSupportObjsOnDeathEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] SupportObjectData[] _createdSupportObjs;
    [SerializeReference] IPattern _objsPattern;
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var createSupportObjOriginal = original as CreateSupportObjsOnDeathEnemyBehaviour;
        _createdSupportObjs = createSupportObjOriginal._createdSupportObjs;
        _objsPattern = createSupportObjOriginal._objsPattern;
    }
    public override void OnDeath()
    {
        base.OnDeath();
        List<Vector2> objsPositions = _objsPattern.GetPositions(EnemyControl.transform.position, _createdSupportObjs.Length).ToList();
        for(int i = 0; i < _createdSupportObjs.Length; i++) 
        {
            Utility.GenerateSupportObj(_createdSupportObjs[i], objsPositions[i], Quaternion.identity);
        }
    }
}
