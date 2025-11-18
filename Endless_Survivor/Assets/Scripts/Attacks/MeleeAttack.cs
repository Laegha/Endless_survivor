using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Attack
{
    public override AttackEffectArea AttackEffectArea
    {
        get { return new AttackEffectArea(AttackEffectArea.IAttackEffectAreaType.Square, _attackAreaStart, _attackAreaEnd, false); }
    }
    [SerializeField] CustomAnimator _vfxAnimator;
    [SerializeField] SpriteRenderer _vfxRenderer;
    float _knockbackForce = 0f;
    MeleeData _attackData;
    Vector2 _attackAreaStart;
    Vector2 _attackAreaEnd;

    private void Update()
    {
        _vfxRenderer.flipY = ParentWeapon.WeaponControl.Gfx.flipY;
    }
    public void Attack(int damage, float knockbackForce, MeleeData attackData)
    {
        AttackDamage = damage;
        _knockbackForce = knockbackForce;
        _attackData = attackData;
        _attackAreaStart = attackData.IsCircle ? (Vector2)transform.position - Vector2.one.normalized * attackData.CircleRadius : (Vector2)transform.position - attackData.BoxSize/2;
        _attackAreaEnd = attackData.IsCircle ? (Vector2)transform.position + Vector2.one.normalized * attackData.CircleRadius : (Vector2)transform.position + attackData.BoxSize/2;
        
        EffectsHandler.TryEffects(this);
        var attackAnimation = new CustomAnimation(_vfxAnimator, attackData.AttackVfxAnimation);
        attackAnimation.Events.Add(new(null, attackData.DamageFrame, ApplyDamage));
        attackAnimation.Events.Add(new(null, attackAnimation.Frames.Length -1, () => Destroy(gameObject)));
        _vfxAnimator.AddAnimations(new List<CustomAnimation> { attackAnimation });
        _vfxAnimator.ChangeAnim(attackAnimation.AnimationName);
        ParticleConfig particles = new ParticleConfig(attackData.AttackParticles, transform.position, transform.rotation, attackData.ParticleDuration, transform);
        ParticleManager.pm.SpawnParticles(particles);

    }
    void ApplyDamage()
    {
        var attackPos = (Vector2)transform.position + (Vector2)(transform.right * _attackData.AttackOffset.x + transform.up * _attackData.AttackOffset.y * (_vfxRenderer.flipY ? -1 : 1));
        var affectedEnemies = _attackData.IsCircle ? Physics2D.OverlapCircleAll(attackPos, _attackData.CircleRadius, Utility.GetCollidableLayers("PlayerAttack")) : Physics2D.OverlapBoxAll(attackPos, _attackData.BoxSize, transform.rotation.z, Utility.GetCollidableLayers("PlayerAttack"));
        foreach(var enemyCol in affectedEnemies)
        {
            var enemyControl = Utility.FindFirstComponentInParent<EnemyControl>(enemyCol.gameObject);
            if (enemyControl == null)
                continue;
            EffectsHandler.EnemyHit(enemyControl);
            Vector2 hitDirection = (enemyCol.transform.position - PlayerControl.pc.transform.position).normalized;
            enemyControl.EnemyHP.TakeDamage(AttackDamage, hitDirection, _knockbackForce);
        }
    }
}
