using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    List<EnemyStatusEffect> _currentEffects;
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
