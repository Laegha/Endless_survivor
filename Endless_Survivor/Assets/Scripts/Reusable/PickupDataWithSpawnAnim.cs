using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PickupDataWithSpawnAnim
{
    [SerializeField] CustomAnimation _spawnAnim;
    [SerializeField] PickupData _pickupData;

    public void SpawnPickupAfterAnim(Vector2 spawnPos, float delayAfterAnimEnd = 0)
    {
        float animatedObjDuration = _spawnAnim.AnimDuration + delayAfterAnimEnd;
        AnimatedObjConfig animationConfig = new(_spawnAnim, spawnPos, Quaternion.identity, animatedObjDuration, null, false, false);
        AnimatedObjsManager.aom.SpawnAnimatedObj(animationConfig);
        GameManager.gm.DelayAction(animatedObjDuration, () => Utility.GeneratePickup(_pickupData, spawnPos), null);
    }
}
