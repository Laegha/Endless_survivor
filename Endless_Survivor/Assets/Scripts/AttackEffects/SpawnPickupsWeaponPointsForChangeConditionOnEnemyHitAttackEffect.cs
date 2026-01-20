using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnPickupsWeaponPointsForChangeConditionOnEnemyHitAttackEffect : AttackEffect
{
    public SpawnPickupsWeaponPointsForChangeConditionOnEnemyHitAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }
    [SerializeField] ChangeAttackConditionPointsPickupData _spawnedPickupData;
    [SerializeField] bool _throwInRandomDirUponSpawn;
    [SerializeField] AnimationCurve _throwCurve;
    [SerializeField] float _throwDist;
    [SerializeField] float _throwSpeed;

    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var spawnPickupPointsOriginal = original as SpawnPickupsWeaponPointsForChangeConditionOnEnemyHitAttackEffect;
        _spawnedPickupData = spawnPickupPointsOriginal._spawnedPickupData;
        _throwInRandomDirUponSpawn = spawnPickupPointsOriginal._throwInRandomDirUponSpawn;
        _throwCurve = spawnPickupPointsOriginal._throwCurve;
        _throwDist = spawnPickupPointsOriginal._throwDist;
        _throwSpeed = spawnPickupPointsOriginal._throwSpeed;
        OnEnemyHit += Spawn;
    }
    void Spawn(EnemyControl hitEnemy)
    {
        Vector2 initialPos = hitEnemy.transform.position;
        var pickupComponent = Utility.GeneratePickup(_spawnedPickupData, initialPos).Pickup;
        pickupComponent.AddVariable<WeaponAttackManager>(_spawnedPickupData.PickupWeaponId, AffectedAttack.ParentWeapon.WeaponControl.WeaponAttackManager);

        if (!_throwInRandomDirUponSpawn)
            return;
        Vector2 randDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        while (Physics2D.Raycast(initialPos, randDirection, _throwDist, LayerMask.NameToLayer("Map")))
        {
            randDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }
        Vector2 endPosition = initialPos + randDirection * _throwDist;

        TransformMoverWithArc pickupMover = new(initialPos, endPosition, _throwCurve, pickupComponent.transform, _throwSpeed);
        //pickupComponent.AddVariable<TransformMoverWithArc>("pickupMover", pickupMover);

        GameManager.gm.RoutineRunner(UpdateTransformMover(pickupMover));
        //GameManager.gm.RoutineRunner(UpdateTransformMover(pickupComponent.GetVariable<TransformMoverWithArc>("pickupMover")));

    }
    IEnumerator UpdateTransformMover(TransformMoverWithArc mover)
    {
        while(!mover.MovementEnded)
        {
            mover.Move();
            yield return null;
        }
    }
}
