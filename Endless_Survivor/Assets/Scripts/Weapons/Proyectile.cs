using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectile : Attack
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] CapsuleCollider2D _collider;
    float _speed = 1;
    int _damage = 1;
    float _lifeTime = 5;
    float _lapsedTime;
    List<Collider2D> _ignoreColliders = new List<Collider2D>();
    public float Speed { get { return _speed; } set { _speed = value; } }
    public int Damage { get { return _damage; } set { _damage = value; } }
    public SpriteRenderer SpriteRenderer { get { return _spriteRenderer; } }
    public CapsuleCollider2D Collider { get { return _collider; } }
    public float LifeTime {  get { return _lifeTime; } set { _lifeTime = value; } }

    public void Initiate(int damage, float speed, float lifeTime, ProyectileData proyectileData, float proyectileSpread = 0, List<Collider2D> ignoreColliders = null)
    {
        _damage = damage;
        _speed = speed;
        _lifeTime = lifeTime;
        _spriteRenderer.sprite = proyectileData.ProyectileSprite;
        if(proyectileData.ProyectileMaterial != null )
            _spriteRenderer.material = proyectileData.ProyectileMaterial;
        if(proyectileData.ParticlesPrefab != null )
            Instantiate(proyectileData.ParticlesPrefab, transform)?.GetComponent<ParticleSystem>()?.Play();
        if (proyectileData.ShootSFX != null)
        {
            SoundFXManager.sm.PlaySfx(proyectileData.ShootSFX, transform.position);
        }
        _collider.size = proyectileData.ColliderSize;
        if(ignoreColliders == null)
            ignoreColliders = new List<Collider2D>();
        _ignoreColliders = ignoreColliders;
        transform.Rotate(new Vector3(0, 0, Random.Range(-proyectileSpread, proyectileSpread)));

        EffectsHandler.TryEffects(this);

        DamageSource[] damageSources = GetComponents<DamageSource>();
        foreach(DamageSource damageSource in damageSources)
        {
            damageSource.IgnoreColliders = ignoreColliders;
            damageSource.Damage = _damage;
        }
        StartMoving();
        
    }
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
        if (_ignoreColliders.Contains(collider))
            return;
        var enemyControl = collider.GetComponent<EnemyControl>();
        if (enemyControl != null)
            EffectsHandler.EnemyHit(enemyControl);
        Destroy(gameObject);
    }
}
