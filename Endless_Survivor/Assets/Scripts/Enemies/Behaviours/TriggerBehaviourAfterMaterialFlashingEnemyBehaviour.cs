using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBehaviourAfterMaterialFlashingEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] string[] _behavioursToTrigger;
    [SerializeField] MaterialOverride _flashingOverride;
    [SerializeField] float _flashingRate;
    [SerializeField] float _flashingDuration;
    [SerializeField] SFXInfo _onFlashingStartSFX;

    SpriteMaterialFlashing _materialFlashing;

    bool _isFlashing;
    float _timer;

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var triggerAfterFlashingOriginal = original as TriggerBehaviourAfterMaterialFlashingEnemyBehaviour;
        _behavioursToTrigger = triggerAfterFlashingOriginal._behavioursToTrigger;
        _flashingOverride = triggerAfterFlashingOriginal._flashingOverride;
        _flashingRate = triggerAfterFlashingOriginal._flashingRate;
        _flashingDuration = triggerAfterFlashingOriginal._flashingDuration;
        _onFlashingStartSFX = triggerAfterFlashingOriginal._onFlashingStartSFX;

        _materialFlashing = new(EnemyControl.MaterialManager, _flashingRate, new(_flashingOverride.authority, _flashingOverride.material));

    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        if(!_isFlashing)
        {
            _isFlashing = true;
            SoundFXManager.sm.PlaySfx(_onFlashingStartSFX, EnemyControl.transform.position);
            _timer = 0;
            _materialFlashing.Start();
        }
        if(_timer > _flashingDuration)
        {
            KillBehaviour();
            _timer = 0;
            return;
        }
        _timer += Time.deltaTime;
        _materialFlashing.Update();


    }

    public override void KillBehaviour()
    {
        base.KillBehaviour();
        _materialFlashing.End();
        _isFlashing = false; 
        foreach (var behaviour in _behavioursToTrigger)
            EnemyControl.BehaviourManager.ActivateBehaviour(behaviour);

    }
}
