using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticlesOnEnemyHitAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    [SerializeField] ParticleSystem _onEnemyHitParticles;
    [SerializeField] float _particlesDuration;
    public SpawnParticlesOnEnemyHitAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack)
    {

    }
    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var originalSpawnParticles = original as SpawnParticlesOnEnemyHitAttackEffect;
        _onEnemyHitParticles = originalSpawnParticles._onEnemyHitParticles;
        _particlesDuration = originalSpawnParticles._particlesDuration;
        OnEnemyHit += SpawnParticles;
    }
    void SpawnParticles(EnemyControl hitEnemy)
    {
        ParticleConfig particles = new(_onEnemyHitParticles, hitEnemy.transform.position, Quaternion.identity, _particlesDuration);
        ParticleManager.pm.SpawnParticles(particles);
    }
}
