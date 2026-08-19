using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileAttack : Attack
{
    [SerializeField] Rigidbody2D _rb;

    [SerializeField] CustomAnimator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] CapsuleCollider2D _collider;
    float _speed = 1;
    float _lifeTime = 5;
    float _lapsedTime;
    List<Collider2D> _ignoreColliders = new List<Collider2D>();
    bool _phasingMap = false;
    new public AnimationChangeAttackGfxInterface AttackGfxInterface => new AnimationChangeAttackGfxInterface();
    public override AttackEffectArea AttackEffectArea
    {
        get
        {
            return new AttackEffectArea(AttackEffectArea.IAttackEffectAreaType.Point, transform.position, transform.position, true);
        }
    }
    public float Speed { get { return _speed; } set { _speed = value; } }
    public CustomAnimator Animator { get { return _animator; } }
    public CapsuleCollider2D Collider { get { return _collider; } }
    public float LifeTime {  get { return _lifeTime; } set { _lifeTime = value; } }
    public void StartProyectile(Vector2 position, Quaternion rotation)
    {
        var collidingObj = Physics2D.OverlapBox(position, Vector2.one * .1f, 0, LayerMask.GetMask("Map"));
        if (collidingObj != null)
        {
            _ignoreColliders.Add(collidingObj);
            _phasingMap = true;
        }
        transform.position = position;
        transform.rotation = rotation;

    }
    public void Initiate(int damage, float knockback, float speed, float lifeTime, ProyectileData proyectileData, float proyectileSpread = 0, List<Collider2D> ignoreColliders = null)
    {
        AttackDamage = damage;
        AttackKnockback = knockback;
        _speed = speed;
        _lifeTime = lifeTime;
        _animator.AddAnimations(new() { proyectileData.ProyectileAnim });
        _animator.ChangeAnim(proyectileData.ProyectileAnim.AnimationName);
        if(proyectileData.ProyectileMaterial != null )
            _spriteRenderer.material = proyectileData.ProyectileMaterial;
        if(proyectileData.ParticlesPrefab != null )
            Instantiate(proyectileData.ParticlesPrefab, transform)?.GetComponent<ParticleSystem>()?.Play();
        if (proyectileData.ShootSFX != null)
        {
            SoundFXManager.sm.PlaySfx(proyectileData.ShootSFX, transform.position);
        }
        _collider.direction = proyectileData.ColliderDirection;
        _collider.size = proyectileData.ColliderSize;
        if(ignoreColliders == null)
            ignoreColliders = new List<Collider2D>();

        _ignoreColliders.AddRange(ignoreColliders);
        transform.Rotate(new Vector3(0, 0, Random.Range(-proyectileSpread, proyectileSpread)));
        _spriteRenderer.flipY = transform.right.x < 0;

        EffectsHandler.TryEffects(this);

        DamageSource[] damageSources = GetComponents<DamageSource>();
        foreach(DamageSource damageSource in damageSources)
        {
            damageSource.IgnoreColliders = ignoreColliders;
            damageSource.Damage = AttackDamage;
            var enemyDamageSource = damageSource as EnemyDamageSource;
            if(enemyDamageSource != null ) 
                enemyDamageSource.Knockback = AttackKnockback;
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

        if (!_phasingMap)
            return;

        var collidingObj = Physics2D.OverlapBox(transform.position, Vector2.one * .01f, 0, LayerMask.GetMask("Map"));

        if (collidingObj == null)
        {
            _phasingMap = false;
            return;
        }
        if (!_ignoreColliders.Contains(collidingObj))
            _ignoreColliders.Add(collidingObj);

    }

    public void StartMoving()
    {
        _rb.velocity = transform.right * _speed;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (_ignoreColliders.Contains(collider) || _phasingMap && collider.gameObject.layer == LayerMask.NameToLayer("Map"))
            return;
        var enemyControl = Utility.FindFirstComponentInParent<EnemyControl>(collider.gameObject);
        if (enemyControl != null)
        {
            EffectsHandler.EnemyHit(enemyControl);
            Vector2 hitDirection = (enemyControl.transform.position - PlayerControl.pc.transform.position).normalized;
            enemyControl.EnemyHP.TakeDamage(AttackDamage, hitDirection, AttackKnockback);

        }
        else
        {
            HP objHP = Utility.FindFirstComponentInParent<HP>(collider.gameObject);
            if (objHP != null)
                objHP.TakeDamage(AttackDamage);

        } 
        if(AttackPiercing <= 0)
            Destroy(gameObject);
        AttackPiercing--;
    }

    public override void ChangeGfx(AttackGfxInterface gfxInterface)
    {
        AnimationChangeAttackGfxInterface attackAnimInterface = gfxInterface as AnimationChangeAttackGfxInterface;
        //attackAnimInterface.ChangeAttackGfx()
    }
}
