using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSupportObjOnRandomEnemiesOnActivateItemBehaviour : PassiveItemBehaviour
{
    [SerializeField] SupportObjectData _createdSupportObj;
    [SerializeField] RandomBetweenTwoConstants _createdAmmount;
    [SerializeField] bool _objsLockIntoEnemies;
    [SerializeField] Vector2 _lockedObjsOffset;

    List<TransformFollowHandler> _lockedObjsHandlers = new List<TransformFollowHandler>();

    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var createSupportObjsOriginal = original as CreateSupportObjOnRandomEnemiesOnActivateItemBehaviour;
        _createdSupportObj = createSupportObjsOriginal._createdSupportObj;
        _createdAmmount = createSupportObjsOriginal._createdAmmount;
        _objsLockIntoEnemies = createSupportObjsOriginal._objsLockIntoEnemies;
        _lockedObjsOffset = createSupportObjsOriginal._lockedObjsOffset;
        if(_objsLockIntoEnemies)
            behaviourManager.onUpdate += UpdateLockedObjsPositions;
    }

    public override void Activate()
    {
        base.Activate();
        CreateSupportObjs();
    }

    void CreateSupportObjs()
    {
        int creatingAmmount = (int)_createdAmmount.rand;
        for (int i = 0; i < creatingAmmount; i++)
        {
            Transform randomEnemyTr = EnemySpawnManager.esm.Enemies[Random.Range(0, EnemySpawnManager.esm.Enemies.Count)].transform;
            var createdObj = Utility.GenerateSupportObj(_createdSupportObj, randomEnemyTr.position, Quaternion.identity);
            if (_objsLockIntoEnemies)
            {
                var lockedObjHandler = new TransformFollowHandler(createdObj.transform, randomEnemyTr, true, true, _lockedObjsOffset);
                _lockedObjsHandlers.Add(lockedObjHandler);
            }
        }
    }

    void UpdateLockedObjsPositions()
    {
        var lockedObjsHandlersCopy = new List<TransformFollowHandler>(_lockedObjsHandlers);
        foreach (var lockedObj in lockedObjsHandlersCopy)
        {
            if (lockedObj.parent || lockedObj.child == null)
            {
                _lockedObjsHandlers.Remove(lockedObj);
                continue;
            }
            lockedObj.Update();
        }
    }

    public override void RemoveBehaviour()
    {

    }
}
