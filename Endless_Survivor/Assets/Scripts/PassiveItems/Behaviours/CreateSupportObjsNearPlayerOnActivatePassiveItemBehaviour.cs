using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateSupportObjsNearPlayerOnActivatePassiveItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] List<SupportObjectData> _createdSupportObjs;
    [SerializeField] IPattern _createdObjsPattern;
    [SerializeField] RandomBetweenTwoConstants _distanceToPlayer;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var createSupportObjOriginal = original as CreateSupportObjsNearPlayerOnActivatePassiveItemBehaviour;
        _createdSupportObjs = createSupportObjOriginal._createdSupportObjs;
        _createdObjsPattern = createSupportObjOriginal._createdObjsPattern;
        _distanceToPlayer = createSupportObjOriginal._distanceToPlayer;
    }
    public override void Activate()
    {
        base.Activate();
        float creatingDist = _distanceToPlayer.rand;
        var tilesInDist = MapManager.mm.LoadedTiles.Where(tile => Vector2.Distance(tile.transform.position, PlayerControl.pc.transform.position) < creatingDist).ToList();
        if (tilesInDist.Count == 0)
            return;
        Vector2 creatingPos = tilesInDist[Random.Range(0, tilesInDist.Count)].transform.position;
        CreateSupportObjs(creatingPos);

    }

    void CreateSupportObjs(Vector2 creatingPosCenter)
    {
        List<Vector2> positions = _createdObjsPattern.GetPositions(creatingPosCenter, _createdSupportObjs.Count).ToList();
        var shuflledObjs = Utility.ShuffleList(_createdSupportObjs);
        for(int i = 0; i < positions.Count; i++)
        {
            Utility.GenerateSupportObj(shuflledObjs[i], positions[i], Quaternion.identity);
        }
    }

    public override void RemoveBehaviour()
    {

    }
}
