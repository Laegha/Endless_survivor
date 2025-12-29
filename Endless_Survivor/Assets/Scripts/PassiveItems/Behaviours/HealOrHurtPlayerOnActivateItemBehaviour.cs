using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOrHurtPlayerOnActivateItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] RouletteElementChance<int> _heal;
    [SerializeField] ParticleSystem _healParticles;
    [SerializeField] RouletteElementChance<int> _hurt;
    [SerializeField] ParticleSystem _hurtParticles;
    [SerializeField] Vector2 _particlesOffsetFromPlayer;
    [SerializeField] float _particlesDuration = 1f;
    Roulette<RouletteElementChance<int>> _healOrHurtRoulette;

    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var healOrHurtOriginal = original as HealOrHurtPlayerOnActivateItemBehaviour;
        _heal = new(healOrHurtOriginal._heal.Element, healOrHurtOriginal._heal.Chance);
        _hurt = new(healOrHurtOriginal._hurt.Element, healOrHurtOriginal._hurt.Chance);

        Dictionary<RouletteElementChance<int>, int> rouletteElements = new()
        {
            { _heal, _heal.Chance },
            { _hurt, _hurt.Chance }
        };
        _healOrHurtRoulette = new(rouletteElements);
    }
    public override void Activate()
    {
        base.Activate();

        RouletteElementChance<int> healOrHurt = _healOrHurtRoulette.Spin();

        if (healOrHurt == _heal)
        {
            PlayerControl.pc.PlayerHPManager.Heal(_heal.Element);
            GenerateParticles(_healParticles);
        }
        else
        {
            PlayerControl.pc.PlayerHPManager.TakeDamage(_hurt.Element);
            GenerateParticles(_hurtParticles);
        }
    }
    void GenerateParticles(ParticleSystem particles)
    {
        ParticleConfig particleConfig = new(particles, (Vector2)PlayerControl.pc.transform.position + _particlesOffsetFromPlayer, Quaternion.identity, _particlesDuration, PlayerControl.pc.transform, true, false);
        ParticleManager.pm.SpawnParticles(particleConfig);
    }
}

