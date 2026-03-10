using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [SerializeField] EnemyHP _enemyHP;
    [SerializeField] CapsuleCollider2D _capsuleCollider;
    [SerializeField] CustomAnimator _customAnimator;
    [SerializeField] SpriteRenderer[] _renderers;
    [SerializeField] RendererSortingByY _rendererSorter;
    [SerializeField] EnemyBehaviourManager _behaviourManager;
    [SerializeField] RbForcesController _rbForcesController;
    [SerializeField] EnemyStatusEffectManager _statusEffectManager;
    [SerializeField] MaterialManager _materialManager;
    EnemyData _enemyData;

    public EnemyHP EnemyHP { get { return _enemyHP; } }
    public CapsuleCollider2D CapsuleCollider { get { return _capsuleCollider; } }
    public CustomAnimator Animator { get { return _customAnimator; } }
    public SpriteRenderer[] Renderers { get { return _renderers; } }
    public RendererSortingByY RendererSorter { get { return _rendererSorter; } }
    public EnemyBehaviourManager BehaviourManager { get { return _behaviourManager; } }
    public RbForcesController RbForcesController { get { return _rbForcesController; } }
    public EnemyStatusEffectManager StatusEffectManager {  get { return _statusEffectManager; } }
    public MaterialManager MaterialManager { get { return _materialManager; } }
    public EnemyData EnemyData {  get { return _enemyData; } set { _enemyData = value; } }

    void Update()
    {
        //foreach (EnemyStatusEffect effect in _currentEffects)
        //{
            //effect.Update(); 
        //}
    }
}
