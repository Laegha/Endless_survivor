using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SettingsJsonInfo
{
    public enum ControlsType
    {
        StaticJoystic,
        DynamicJoystic,
        Joystickless
    }

    public float master;
    public float sfx;
    public float music;
    public ControlsType controlsType;
    public bool crt;
    public string selectedCharFileName;
}
