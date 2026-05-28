using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePickupNearPlayerPeriodicallyItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] List<RouletteElementChance<PickupDataWithSpawnAnim>> _possibleCreatedPickup;
    [SerializeField] float _timeBetweenSpawns;
    [Range(0, 100)][SerializeField] float _chanceOfSpawning;
    [SerializeField] float _distFromPlayer = 1f;

    float _lapsedTime;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var createPickupOriginal = original as CreatePickupNearPlayerPeriodicallyItemBehaviour;
        _possibleCreatedPickup = new(createPickupOriginal._possibleCreatedPickup);
        _timeBetweenSpawns = createPickupOriginal._timeBetweenSpawns;
        _chanceOfSpawning = createPickupOriginal._chanceOfSpawning;
        _distFromPlayer = createPickupOriginal._distFromPlayer;
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
        Vector2 itemPosOffset = new(Random.Range(0, 1f), Random.Range(0, 1f));
        itemPosOffset = itemPosOffset.normalized * _distFromPlayer;
        Vector2 itemPos = (Vector2)PlayerControl.pc.transform.position + itemPosOffset;
        Utility.GetRouletteElement(_possibleCreatedPickup).SpawnPickupAfterAnim(itemPos);
    }
    public override void RemoveBehaviour()
    {

    }
}