using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSupportObjsOnPlayerPositionOnActiveEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] List<SupportObjectData> _fixedSupportObjs;
    [SerializeField] List<RouletteElementChance<SupportObjectData>> _randomSupportObjs;
    [Tooltip("Ammount of objects spawned from the random list")][SerializeField] RandomBetweenTwoConstants _randomAmmount;
    [SerializeField] RandomBetweenTwoConstants _distFromPlayerWithinCircle;
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var createSupportObjOnPlayerOriginal = original as CreateSupportObjsOnPlayerPositionOnActiveEnemyBehaviour;
        _fixedSupportObjs = createSupportObjOnPlayerOriginal._fixedSupportObjs;
        _randomSupportObjs = createSupportObjOnPlayerOriginal._randomSupportObjs;
        _randomAmmount = createSupportObjOnPlayerOriginal._randomAmmount;
        _distFromPlayerWithinCircle = createSupportObjOnPlayerOriginal._distFromPlayerWithinCircle;
    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        foreach(SupportObjectData supportObj in _fixedSupportObjs)
        {
            CreateSupportObj(supportObj);
        }
        if(_randomSupportObjs.Count > 0)
        {
            int generatedAmmount = (int)_randomAmmount.rand;
            for(int i = 0; i < generatedAmmount; i++)
            {
                CreateSupportObj(Utility.GetRouletteElement(_randomSupportObjs));
            }
        }

        KillBehaviour();
    }

    void CreateSupportObj(SupportObjectData supportObjData)
    {
        Vector2 objPosition = (Vector2)PlayerControl.pc.transform.position + Random.insideUnitCircle * _distFromPlayerWithinCircle.rand;
        Utility.GenerateSupportObj(supportObjData, objPosition, Quaternion.identity);
    }
}
