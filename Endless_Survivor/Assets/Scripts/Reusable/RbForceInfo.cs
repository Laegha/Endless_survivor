using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RbForceInfo
{
    public static int maxPriority = 100;
    public Vector2 direction;
    public float strength;
    public int priority;
    public ForceMode2D forceMode;
    public float impulseTime;

    public RbForceInfo(Vector2 direction, float strength, int priority, ForceMode2D forceMode, float impulseTime = 0)
    {
        this.direction = direction;
        this.strength = strength;
        this.priority = priority > maxPriority ? maxPriority : priority;
        this.forceMode = forceMode;
        this.impulseTime = impulseTime;
    }
}
