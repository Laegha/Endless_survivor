using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActivateRandomPeriodicallyItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] List<RouletteElementChance<string>> _possibleActivatedIds;
    [SerializeField] RandomBetweenTwoConstants _timeBetweenActivations;
    [SerializeField] ParticleSystem _particlesOnActivation;
    [SerializeField] Vector2 _particlesOffsetFromPlayer;
    [SerializeField] float _particlesDuration;
    [SerializeField] SFXInfo _onActivateSFX;
    float _activationTimer;

    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var activateRandOriginal = original as ActivateRandomPeriodicallyItemBehaviour;
        _possibleActivatedIds = new(activateRandOriginal._possibleActivatedIds);
        _timeBetweenActivations = activateRandOriginal._timeBetweenActivations;
        _particlesOnActivation = activateRandOriginal._particlesOnActivation;
        _particlesOffsetFromPlayer = activateRandOriginal._particlesOffsetFromPlayer;
        _particlesDuration = activateRandOriginal._particlesDuration;
        _onActivateSFX = activateRandOriginal._onActivateSFX;

        ResetTimer();
        behaviourManager.onUpdate += ReduceActivationTimer;
    }
    void ReduceActivationTimer()
    {
        _activationTimer -= Time.deltaTime;
        if(_activationTimer < 0)
        {
            ResetTimer();
            ActivateRandomBehaviour();
        }
    }
    void ResetTimer()
    {
        _activationTimer = _timeBetweenActivations.rand;
    }
    void ActivateRandomBehaviour()
    {
        string activatedId = Utility.GetRouletteElement(_possibleActivatedIds);
        var activatedBehaviour = BehaviourManager.ItemBehaviours.Find(x => x.BehaviourId == activatedId);
        activatedBehaviour.Activate();
        SoundFXManager.sm.PlaySfx(_onActivateSFX, PlayerControl.pc.transform.position);
        if (_particlesOnActivation == null)
            return;
        ParticleConfig particleConfig = new(_particlesOnActivation,(Vector2)PlayerControl.pc.transform.position + _particlesOffsetFromPlayer, Quaternion.identity, _particlesDuration);
        ParticleManager.pm.SpawnParticles(particleConfig);
    }
    public override void RemoveBehaviour()
    {

    }
}
