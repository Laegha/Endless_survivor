using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "ScriptableObjects/Enemy", order = 3)]
public class EnemyData : ScriptableObject
{
    [SerializeField] int _initialHP;
    [SerializeField] float _knockbackResistance;
    [SerializeField] Vector2 _colliderSize;
    [SerializeField] Vector2 _colliderOffset;
    [SerializeField] CapsuleDirection2D _colliderDirection;
    [SerializeReference] List<EnemyBehaviour> _enemyBehaviours = new List<EnemyBehaviour>();
    [SerializeField] List<PickupDataChance> _dropablePickupChances = new List<PickupDataChance>();

    [SerializeField] SFXInfo _onHitSFX;
    [SerializeField] SFXInfo _onDeathSFX;

    public Vector2 ColliderSize {  get { return _colliderSize; } }
    public List<EnemyBehaviour> EnemyBehaviours { get { return _enemyBehaviours; }set { _enemyBehaviours = value; } }
    public List<PickupDataChance> DropablePickupChances { get { return _dropablePickupChances; } }

    public void TransferEnemyData(GameObject enemy)
    {
        EnemyControl enemyControl = enemy.GetComponent<EnemyControl>();
        EnemyHP enemyHP = enemyControl.EnemyHP;
        enemyHP.InitializeHP(_initialHP + ScalingFunctions.EnemyHPIncrease(ScalingFunctions.CurrWaveLevel));
        enemyHP.SetSounds(_onHitSFX, _onDeathSFX);
        enemyHP.DropablePickupChances = new List<PickupDataChance>(_dropablePickupChances);
        enemyHP.KnockbackResistance = _knockbackResistance;

        enemyControl.CapsuleCollider.direction = _colliderDirection;
        enemyControl.CapsuleCollider.size = _colliderSize;
        enemyControl.CapsuleCollider.offset = _colliderOffset;

        foreach(var enemyBehaviour in _enemyBehaviours)
        {
            if (enemyBehaviour == null)
                continue;
            enemyControl.BehaviourManager.AddBehaviour(enemyBehaviour, enemyControl);
        }
        enemyControl.BehaviourManager.RewriteAllOverrides();//rewrite overrides so they are pointing to the BehaviourManager behaviours instead of EnemyData behaviours
    }
}
