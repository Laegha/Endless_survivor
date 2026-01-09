using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEnemyOnHit : AttackEffect
{
    new public static bool isUsable => true;
    [SerializeField] float _minDistFromPlayer;
    [SerializeField] ParticleSystem _teleportParticles;
    [SerializeField] float _particlesDuration;
    public TeleportEnemyOnHit(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }
    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var teleportEnemyOriginal = original as TeleportEnemyOnHit;
        _minDistFromPlayer = teleportEnemyOriginal._minDistFromPlayer;
        _teleportParticles = teleportEnemyOriginal._teleportParticles;
        _particlesDuration = teleportEnemyOriginal._particlesDuration;
        OnEnemyHit += TeleportEnemy;
    }
    void TeleportEnemy(EnemyControl hitEnemy)
    {
        Vector2 enemyPos = Utility.GetRandomPosInMap();
        while (Vector2.Distance(enemyPos, PlayerControl.pc.transform.position) < _minDistFromPlayer)
        {
            enemyPos = Utility.GetRandomPosInMap();
        }
        ParticleConfig particleConfig = new(_teleportParticles, hitEnemy.transform.position, Quaternion.identity, _particlesDuration, null, false, false);
        ParticleManager.pm.SpawnParticles(particleConfig);
        hitEnemy.transform.position = enemyPos;
    }
}
