using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RandomBetweenTwoConstants
{
    public float min;
    public float max;
    public float rand => UnityEngine.Random.Range(min, max);
}
