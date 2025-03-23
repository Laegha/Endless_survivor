using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectile : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] CapsuleCollider2D _collider;
    float _speed = 1;
    int _damage = 1;
    float _lifeTime = 5;
    float _lapsedTime;
    public float Speed { get { return _speed; } set { _speed = value; } }
    public int Damage { get { return _damage; } set { _damage = value; } }
    public SpriteRenderer SpriteRenderer { get { return _spriteRenderer; } }
    public CapsuleCollider2D Collider { get { return _collider; } }
    public float LifeTime {  get { return _lifeTime; } set { _lifeTime = value; } }

    private void Update()
    {
        _lapsedTime += Time.deltaTime;
        if(_lapsedTime > _lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public void StartMoving()
    {
        _rb.velocity = -transform.right * Speed;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        EnemyControl hitEnemyControl = collider.GetComponent<EnemyControl>();
        if(hitEnemyControl != null)
            hitEnemyControl.EnemyHP.RecieveDamage(_damage);
        Destroy(gameObject);
    }
}
