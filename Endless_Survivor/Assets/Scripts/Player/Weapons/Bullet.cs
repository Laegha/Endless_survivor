using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] CapsuleCollider2D _collider;
    float _speed = 1;
    public float Speed { get { return _speed; } set { _speed = value; } }
    public SpriteRenderer SpriteRenderer { get { return SpriteRenderer; } }
    public CapsuleCollider2D Collider { get { return _collider; } }
    private void StartMoving()
    {
        _rb.velocity = transform.forward * Speed;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        EnemyControl hitEnemyControl = collider.GetComponent<EnemyControl>();
    }
}
