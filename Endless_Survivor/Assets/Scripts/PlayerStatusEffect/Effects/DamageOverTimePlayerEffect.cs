using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimePlayerEffect : PlayerStatusEffect
{
    new public static int maxStacks => -1;
    [SerializeField] int _damage;
    [SerializeField] float _damageFrequency = .5f;
    float _damageTimer = 0;
    public override void Initialize(PlayerStatusEffect original)
    {
        base.Initialize(original);
        DamageOverTimePlayerEffect damageOverTimeOriginal = original as DamageOverTimePlayerEffect;
        _damage = damageOverTimeOriginal._damage;
        _damageFrequency = damageOverTimeOriginal._damageFrequency;
    }
    public override void Update()
    {
        base.Update();
        _damageTimer += Time.deltaTime;
        if (_damageTimer >= _damageFrequency)
        {
            PlayerControl.pc.PlayerHPManager.TakeDamage(_damage);
            _damageTimer = 0;
        }
    }
}
