using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [SerializeField] EnemyHP _enemyHP;
    [SerializeField] CapsuleCollider2D _capsuleCollider;
    [SerializeField] CustomAnimator _customAnimator;
    [SerializeField] EnemyBehaviourManager _behaviourManager;
    [SerializeField] RbForcesController _rbForcesController;
    [SerializeField] EnemyStatusEffectManager _statusEffectManager;
    [SerializeField] MaterialManager _materialManager;

    public EnemyHP EnemyHP { get { return _enemyHP; } }
    public CapsuleCollider2D CapsuleCollider { get { return _capsuleCollider; } }
    public CustomAnimator Animator { get { return _customAnimator; } }
    public EnemyBehaviourManager BehaviourManager { get { return _behaviourManager; } }
    public RbForcesController RbForcesController { get { return _rbForcesController; } }
    public EnemyStatusEffectManager StatusEffectManager {  get { return _statusEffectManager; } }
    public MaterialManager MaterialManager { get { return _materialManager; } }

    void Update()
    {
        //foreach (EnemyStatusEffect effect in _currentEffects)
        //{
            //effect.Update(); 
        //}
    }
}
