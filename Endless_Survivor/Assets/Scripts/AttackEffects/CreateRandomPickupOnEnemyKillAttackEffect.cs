using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRandomPickupOnEnemyKillAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    [SerializeField] List<RouletteElementChance<PickupData>> _possiblePickups;
    [SerializeField] Vector2 _createdPickupOffset;
    static List<EnemyControl> _addedEnemies = new List<EnemyControl>();
    public CreateRandomPickupOnEnemyKillAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }

    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var createPikcupOriginal = original as CreateRandomPickupOnEnemyKillAttackEffect;
        _possiblePickups = new(createPikcupOriginal._possiblePickups);
        _createdPickupOffset = createPikcupOriginal._createdPickupOffset;
        OnEnemyHit += AddActionToEnemyDeath;
    }

    void AddActionToEnemyDeath(EnemyControl hitEnemy)
    {
        if (_addedEnemies.Contains(hitEnemy)) return;
        hitEnemy.EnemyHP.OnDeath += CreateSupportObjOnEnemyPos;
        _addedEnemies.Add(hitEnemy);
    }

    void CreateSupportObjOnEnemyPos(EnemyControl hitEnemy)
    {
        var createdPickup = Utility.GetRouletteElement(_possiblePickups);
        Utility.GeneratePickup(createdPickup, (Vector2)hitEnemy.transform.position + _createdPickupOffset);//CONSIDER IF THE POSITION IS OUT OF MAP BOUNDS
        _addedEnemies.Remove(hitEnemy);
    }
}
