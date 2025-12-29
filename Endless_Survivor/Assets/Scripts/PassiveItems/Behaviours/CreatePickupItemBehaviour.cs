using System.Collections.Generic;
using UnityEngine;

public class CreatePickupItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [Range(0, 100)][SerializeField] float _dropChance;
    [SerializeField] List<RouletteElementChance<PickupData>> _possiblePickups;
    [SerializeField] bool _createOnEnemyKill;

    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        CreatePickupItemBehaviour createPickupOriginal = original as CreatePickupItemBehaviour;
        _dropChance = createPickupOriginal._dropChance;
        _possiblePickups = createPickupOriginal._possiblePickups;
        _createOnEnemyKill = createPickupOriginal._createOnEnemyKill;

        if (_createOnEnemyKill)
            behaviourManager.onEnemyKilled += CreatePickupOnEnemyKill;
    }

    void CreatePickupOnEnemyKill(EnemyControl killedEnemy)
    {
        float rand = Random.Range(0, 100);
        if(_dropChance < rand)
            return;
        var dropedPickup = Utility.GetRouletteElement(_possiblePickups);
        Utility.GeneratePickup(dropedPickup, killedEnemy.transform.position);
        Debug.Log("Gwenerating " + dropedPickup);
    }
}
