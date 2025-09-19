using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class KnockbackUtility
{
    public static float knockbackMapDivisor = 1;
    static readonly float _knockbackForceTime = .01f;
    static readonly float _knockbackForceTimeMax = .1f;
    public static RbForceInfo GetKnockbackForceInfo(Vector2 knockbackdirection, float knockbackPower)
    {
        var pushTime =  Mathf.Clamp(_knockbackForceTime * knockbackPower, 0, _knockbackForceTimeMax);
        return new RbForceInfo(knockbackdirection, knockbackPower / knockbackMapDivisor, RbForceInfo.maxPriority, ForceMode2D.Impulse, pushTime);
    }
}
