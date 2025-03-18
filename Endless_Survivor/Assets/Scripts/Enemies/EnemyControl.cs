using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] EnemyHP _enemyHP;
    [SerializeField] CapsuleCollider2D _capsuleCollider;
    [SerializeField] CustomAnimator _customAnimator;
    List<EnemyStatusEffect> _currentEffects = new List<EnemyStatusEffect>();


    public EnemyHP EnemyHP { get { return _enemyHP; } }
    public CapsuleCollider2D CapsuleCollider { get { return _capsuleCollider; } }
    public CustomAnimator CustomAnimator { get { return _customAnimator; } }

    void Update()
    {
        foreach (EnemyStatusEffect effect in _currentEffects)
        {
            effect.Update(); 
        }
    }
}
