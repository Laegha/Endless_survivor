using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "ScriptableObjects/Enemy", order = 3)]
public class EnemyData : ScriptableObject
{
    [SerializeField] int _initialHP;
    [SerializeField] Sprite _referenceSizeSprite;
    [SerializeField] float _knockbackResistance;
    [SerializeField] Vector2 _colliderSize;
    [SerializeField] Vector2 _colliderOffset;
    [SerializeField] CapsuleDirection2D _colliderDirection;
    [SerializeReference] List<EnemyBehaviour> _enemyBehaviours = new List<EnemyBehaviour>();
    [SerializeField] List<RouletteElementChance<PickupData>> _dropablePickupChances = new List<RouletteElementChance<PickupData>>();

    [SerializeField] SFXInfo _onHitSFX;
    [SerializeField] SFXInfo _onDeathSFX;

    readonly static Vector2 _defaultEnemySize = new Vector2(1f, 1f);

    public Vector2 ColliderSize {  get { return _colliderSize; } }
    public List<EnemyBehaviour> EnemyBehaviours { get { return _enemyBehaviours; }set { _enemyBehaviours = value; } }
    public List<RouletteElementChance<PickupData>> DropablePickupChances { get { return _dropablePickupChances; } }

    public void TransferEnemyData(GameObject enemy)
    {
        EnemyControl enemyControl = enemy.GetComponent<EnemyControl>();
        EnemyHP enemyHP = enemyControl.EnemyHP;
        enemyHP.InitializeHP(_initialHP + ScalingFunctions.EnemyHPIncrease(ScalingFunctions.CurrScalingLevel));
        enemyHP.SetSounds(_onHitSFX, _onDeathSFX);
        enemyHP.DropablePickupChances = new List<RouletteElementChance<PickupData>>(_dropablePickupChances);
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

        var enemySize = _referenceSizeSprite != null ? (Vector2)_referenceSizeSprite.bounds.size : _defaultEnemySize;
        Vector2 gridPos =new Vector2(0, enemySize.y / 2 - (_referenceSizeSprite != null ? _referenceSizeSprite.bounds.center.y : 0));
        enemyControl.StatusEffectManager.SetGridLocalPos(gridPos);
    }
}
