using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] EnemyHP _enemyHP;
    List<EnemyStatusEffect> _currentEffects = new List<EnemyStatusEffect>();

    public EnemyHP EnemyHP { get { return _enemyHP; } }
    void Start()
    {
        
    }

    void Update()
    {
        foreach (EnemyStatusEffect effect in _currentEffects)
        {
            effect.Update(); 
        }
    }
}
