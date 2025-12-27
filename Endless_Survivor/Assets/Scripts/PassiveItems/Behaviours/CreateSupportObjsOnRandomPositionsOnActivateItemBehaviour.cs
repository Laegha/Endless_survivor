using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSupportObjsOnRandomPositionsOnActivateItemBehaviour : PassiveItemBehaviour
{
    [SerializeField] List<RouletteElementChance<SupportObjectData>> _possibleSupportObjs;
    [SerializeField] RandomBetweenTwoConstants _ammountPerSpawn;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var createSupportObjsOriginal = original as CreateSupportObjsOnRandomPositionsOnActivateItemBehaviour;
        _possibleSupportObjs = new(createSupportObjsOriginal._possibleSupportObjs);
        _ammountPerSpawn = createSupportObjsOriginal._ammountPerSpawn;
    }
    public override void Activate()
    {
        base.Activate();
        var objsCreated = _ammountPerSpawn.rand;
        for(int i = 0; i < objsCreated; i++)
        {
            Vector2 createdObjPosition = Utility.GetRandomPosInMap();
            SupportObjectData createdObj = Utility.GetRouletteElement(_possibleSupportObjs);
            Utility.GenerateSupportObj(createdObj, createdObjPosition, Quaternion.identity);
        }
    }
}
