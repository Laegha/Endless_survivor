using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSupportObjsOnRandomPositionsOnActivateItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] List<RouletteElementChance<SupportObjectData>> _possibleSupportObjs;
    [SerializeField] RandomBetweenTwoConstants _ammountPerSpawn;
    List<GameObject> _generatedObjs = new List<GameObject>();
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
            var generatedObj = Utility.GenerateSupportObj(createdObj, createdObjPosition, Quaternion.identity);
            _generatedObjs.Add(generatedObj.transform.root.gameObject);
        }
    }
    public override void RemoveBehaviour()
    {
        foreach(var obj in _generatedObjs)
        {
            if (obj == null)
                continue;
            GameObject.Destroy(obj);
        }
    }
}
