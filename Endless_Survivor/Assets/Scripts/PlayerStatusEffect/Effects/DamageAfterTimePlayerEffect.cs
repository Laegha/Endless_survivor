using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAfterTimePlayerEffect : PlayerStatusEffect
{
    new public static int maxStacks => -1;
    [SerializeField] int _damage;

    public override void Initialize(PlayerStatusEffect original)
    {
        base.Initialize(original);
        DamageAfterTimePlayerEffect damageAfterTimeOriginal = original as DamageAfterTimePlayerEffect;
        _damage = damageAfterTimeOriginal._damage;
    }

    public override void End()
    {
        base.End();
        PlayerControl.pc.PlayerHPManager.TakeDamage(_damage);
    }
}
