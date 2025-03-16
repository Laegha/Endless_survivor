using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "ScriptableObjects/Enemy", order = 3)]
public class EnemyData : ScriptableObject
{
    [SerializeField] int _initialHP;
    [SerializeField] Vector2 _colliderSize;
    [SerializeField] CapsuleDirection2D _colliderDirection;
    [SerializeReference] List<EnemyBehaviour> _enemyBehaviours;

    public int InitialHP { get { return _initialHP; } }
    public Vector2 ColliderSize {  get { return _colliderSize; } }
    public List<EnemyBehaviour> EnemyBehaviours { get { return _enemyBehaviours; } }

    public virtual void TransferEnemyData(GameObject enemy)
    {
        EnemyControl enemyControl = enemy.GetComponent<EnemyControl>();
        enemyControl.EnemyHP.LeftHP = _initialHP + WaveManager.wm.LapsedWaves * 3;
        enemyControl.CapsuleCollider.direction = _colliderDirection;
        enemyControl.CapsuleCollider.size = _colliderSize;
        foreach(var enemyBehaviour in _enemyBehaviours)
        {
            enemyBehaviour.TransferData(enemyControl);
        }
    }
}
