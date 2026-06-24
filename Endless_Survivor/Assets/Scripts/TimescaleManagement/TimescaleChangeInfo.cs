using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TimescaleChangeInfo
{
    [SerializeField] float _newTimescale;
    [SerializeField] bool _endByTime;
    [SerializeField] float _changeDuration;
    Action _onChangeEnded;

    float _timer;

    public float NewTimescale { get { return _newTimescale; } }
    public bool EndByTime {  get { return _endByTime; } }
    public Action OnChangeEnded { get { return _onChangeEnded; } }

    public TimescaleChangeInfo(TimescaleChangeInfo original)
    {
        SetValues(original._newTimescale, original._endByTime, original._changeDuration, original._onChangeEnded);
    }
    public TimescaleChangeInfo(float newTimescale, bool endByTime, float changeDuration, Action onChangeEnded = null)
    {
        SetValues(newTimescale, endByTime, changeDuration, onChangeEnded);
    }
    void SetValues(float newTimescale, bool endByTime, float changeDuration, Action onChangeEnded = null)
    {
        _newTimescale = newTimescale;
        _endByTime = endByTime;
        _changeDuration = changeDuration;
        _onChangeEnded = onChangeEnded;
        _timer = _changeDuration;
    }

    public bool HasEnded()
    {
        if(_endByTime)
            _timer -= Time.unscaledDeltaTime;
        return _timer <= 0 && _endByTime;
    }
}
