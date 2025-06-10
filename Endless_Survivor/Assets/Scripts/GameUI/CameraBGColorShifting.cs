using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBGColorShifting : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] Color _sampleColor;
    [SerializeField] float _cycleTime = 2.5f;
    float _cycleSpeed;
    float _cycleTimer;
    float saturation;
    float value;

    void Awake()
    {
        _cycleSpeed = 360 / _cycleTime;
        Color.RGBToHSV(_sampleColor, out _, out saturation, out value);
    }

    void Update()
    {
        print(saturation);
        print(value);
        _cycleTimer += Time.deltaTime;
        if(_cycleTimer >= _cycleTime)
            _cycleTimer = 0;
        float hue = _cycleSpeed * _cycleTimer;
        _camera.backgroundColor = Color.HSVToRGB(hue / 360, saturation, value);
    }
}
