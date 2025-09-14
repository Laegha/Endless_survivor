using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeAttack : Attack
{
    public override AttackEffectArea AttackEffectArea
    {
        get { return new AttackEffectArea(AttackEffectArea.IAttackEffectAreaType.Square, _attackAreaStart, _attackAreaEnd, false); }
    }
    [SerializeField] CustomAnimator _vfxAnimator;
    [SerializeField] SpriteRenderer _vfxRenderer;
    MeleeData _attackData;
    Vector2 _attackAreaStart;
    Vector2 _attackAreaEnd;
    float _circleRadius => Mathf.Clamp(_attackData.CircleRadius, ParentWeapon.WeaponStats.Range, Mathf.Infinity);
    Vector2 _boxSize => _attackData.BoxSize.magnitude >= ParentWeapon.WeaponStats.Range ? _attackData.BoxSize : _attackData.BoxSize.normalized * ParentWeapon.WeaponStats.Range;

    private void Update()
    {
        _vfxRenderer.flipY = ParentWeapon.WeaponControl.Gfx.flipY;
    }
    public void Attack(int damage, MeleeData attackData)
    {
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
        var affectedEnemies = _attackData.IsCircle ? Physics2D.OverlapCircleAll(transform.position, _circleRadius, Utility.GetCollidableLayers("PlayerAttack")) : Physics2D.OverlapBoxAll(transform.position, _boxSize, transform.rotation.z, Utility.GetCollidableLayers("PlayerAttack"));
        foreach(var enemyCol in affectedEnemies)
        {
            var enemyControl = Utility.FindFirstComponentInParent<EnemyControl>(enemyCol.gameObject);
            if (enemyControl == null)
                continue;
            EffectsHandler.EnemyHit(enemyControl);
            enemyControl.EnemyHP.TakeDamage(AttackDamage);
        }
    }
}
