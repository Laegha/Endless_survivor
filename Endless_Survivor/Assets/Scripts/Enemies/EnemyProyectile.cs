using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProyectile : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] CustomAnimator _animator;
    [SerializeField] CapsuleCollider2D _collider;
    float _speed = 1;
    float _lifeTime = 5;
    float _lapsedTime;
    System.Action<PlayerControl> _onPlayerHit;

    public void Initiate(int damage, float lifeTime, ProyectileData proyectileData, System.Action<PlayerControl> onPlayerHit = null)
    {
        _speed = proyectileData.ProyectileSpeed;
        _lifeTime = lifeTime;
        _animator.AddAnimations(new() { proyectileData.ProyectileAnim });
        _animator.ChangeAnim(proyectileData.ProyectileAnim.AnimationName);
        if (proyectileData.ProyectileMaterial != null)
            _spriteRenderer.material = proyectileData.ProyectileMaterial;
        if (proyectileData.ParticlesPrefab != null)
            Instantiate(proyectileData.ParticlesPrefab, transform)?.GetComponent<ParticleSystem>()?.Play();
        if (proyectileData.ShootSFX != null)
        {
            SoundFXManager.sm.PlaySfx(proyectileData.ShootSFX, transform.position);
        }
        _collider.direction = proyectileData.ColliderDirection;
        _collider.size = proyectileData.ColliderSize;

        transform.Rotate(new Vector3(0, 0, Random.Range(-proyectileData.ProyectileSpread, proyectileData.ProyectileSpread)));

        _spriteRenderer.flipY = transform.right.x < 0;

        _onPlayerHit += onPlayerHit;
        DamageSource[] damageSources = GetComponents<DamageSource>();
        foreach (DamageSource damageSource in damageSources)
        {
            damageSource.Damage = damage;
        }
        StartMoving();

    }
    private void Update()
    {
        _lapsedTime += Time.deltaTime;
        if (_lapsedTime > _lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public void StartMoving()
    {
        _rb.velocity = transform.right * _speed;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerControl hitPlayer = Utility.FindFirstComponentInParent<PlayerControl>(collider.gameObject);
        if(hitPlayer != null)
        {
            _onPlayerHit?.Invoke(hitPlayer);
        }
        Destroy(gameObject);
    }

    public void GetParried(CustomAnimation parriedAnimation)
    {
        if (GetComponent<EnemyDamageSource>() != null)
            return;
        GetComponent<Rigidbody2D>().velocity *= -1;
        int proyectileDamage = GetComponent<PlayerDamageSource>().Damage;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 180);
        _spriteRenderer.flipY = !_spriteRenderer.flipY;
        gameObject.AddComponent<EnemyDamageSource>().Damage = proyectileDamage;
        //add animated obj in proyectile
        if (parriedAnimation != null && parriedAnimation.Frames.Length > 0)
        {
            AnimatedObjConfig proyectileAnimConfig = new(parriedAnimation, Vector2.zero, Quaternion.identity, -1, transform);
            var animatedObj = AnimatedObjsManager.aom.SpawnAnimatedObj(proyectileAnimConfig);
            animatedObj.GetComponent<RendererSortingByY>().Offset = 10;
        }
        gameObject.layer = LayerMask.NameToLayer("PlayerAttack");
    }
}
