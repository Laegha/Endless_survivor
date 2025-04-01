using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "ScriptableObjects/Enemy", order = 3)]
public class EnemyData : ScriptableObject
{
    [SerializeField] int _initialHP;
    [SerializeField] Vector2 _colliderSize;
    [SerializeField] Vector2 _colliderOffset;
    [SerializeField] CapsuleDirection2D _colliderDirection;
    [SerializeReference] List<EnemyBehaviour> _enemyBehaviours;
    [SerializeField] List<PickupData> _dropablePickups;
    Dictionary<PickupData, int> _dropablePickupsChances = new Dictionary<PickupData, int>();

    public int InitialHP { get { return _initialHP; } }
    public Vector2 ColliderSize {  get { return _colliderSize; } }
    public List<EnemyBehaviour> EnemyBehaviours { get { return _enemyBehaviours; } }
    public List<PickupData> DropablePickups {  get { return _dropablePickups; } }
    public Dictionary<PickupData, int> DropablePickupsChances {  get { return _dropablePickupsChances; } }

    public void TransferEnemyData(GameObject enemy)
    {
        EnemyControl enemyControl = enemy.GetComponent<EnemyControl>();
        EnemyHP enemyHP = enemyControl.EnemyHP;
        enemyHP.LeftHP = _initialHP + ScalingFunctions.EnemyHPIncrease(WaveManager.wm.CurrWave);
        enemyHP.DropablePickupChances = new Dictionary<PickupData, int>(_dropablePickupsChances);

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
