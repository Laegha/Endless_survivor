using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticlesOnAttackAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    [SerializeField] ParticleSystem _onEnemyHitParticles;
    [SerializeField] float _particlesDuration;
    public SpawnParticlesOnAttackAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack)
    {

    }
    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var originalSpawnParticles = original as SpawnParticlesOnAttackAttackEffect;
        _onEnemyHitParticles = originalSpawnParticles._onEnemyHitParticles;
        _particlesDuration = originalSpawnParticles._particlesDuration;
        OnAttack += SpawnParticles;
    }
    void SpawnParticles()
    {
        ParticleConfig particles = new(_onEnemyHitParticles, AffectedAttack.ParentWeapon.WeaponControl.transform.position, Quaternion.identity, _particlesDuration);
        ParticleManager.pm.SpawnParticles(particles);
    }
}
