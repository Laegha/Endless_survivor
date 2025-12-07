using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePickupOnRandomPosPeriodicallyItemBehaviour : PassiveItemBehaviour
{
    [SerializeField] PickupData _createdPickup;
    [SerializeField] float _timeBetweenSpawns;
    [Range(0, 100)][SerializeField] float _chanceOfSpawning;

    float _lapsedTime;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var createSupportObjOriginal = original as CreatePickupOnRandomPosPeriodicallyItemBehaviour;
        _createdPickup = createSupportObjOriginal._createdPickup;
        _timeBetweenSpawns = createSupportObjOriginal._timeBetweenSpawns;
        _chanceOfSpawning = createSupportObjOriginal._chanceOfSpawning;
        behaviourManager.onUpdate += Update;
    }
    void Update()
    {
        _lapsedTime += Time.deltaTime;
        if(_lapsedTime >= _timeBetweenSpawns)
        {
            _lapsedTime = 0;
            float rand = Random.Range(1, 100f);
            if(_chanceOfSpawning >= rand)
                CreatePickup();
        }
    }
    void CreatePickup()
    {
        Vector2 itemPos = Utility.GetRandomPosInMap();
        Utility.GeneratePickup(_createdPickup, itemPos);
    }
}
