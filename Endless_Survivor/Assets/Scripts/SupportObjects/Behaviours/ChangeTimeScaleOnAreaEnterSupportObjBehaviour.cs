using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTimeScaleOnAreaEnterSupportObjBehaviour : UseAreaAroundSupportObjBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] float _timeScale;
    [SerializeField] float _timeScaleDuration;

    bool _inScaledTime = false;
    float _timer;
    float _prevTimeScale;

    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var changeTimeScaleOriginal = original as ChangeTimeScaleOnAreaEnterSupportObjBehaviour;
        _timeScale = changeTimeScaleOriginal._timeScale;
        _timeScaleDuration = changeTimeScaleOriginal._timeScaleDuration;
        OnObjEnterArea += ChangeTimeScale;
        OnUpdate += TimeScaleEndTimer;
    }

    void ChangeTimeScale(GameObject placeholder)
    {
        if (_inScaledTime)
            return;
        _prevTimeScale = Time.timeScale;
        Time.timeScale = _timeScale;
        _inScaledTime = true;
        _timer = 0;
    }
    void TimeScaleEndTimer()
    {
        if (!_inScaledTime)
            return;
        _timer += Time.unscaledDeltaTime;
        if(_timer > _timeScaleDuration)
        {
            Time.timeScale = _prevTimeScale;
            _inScaledTime = false;
        }
    }
}
