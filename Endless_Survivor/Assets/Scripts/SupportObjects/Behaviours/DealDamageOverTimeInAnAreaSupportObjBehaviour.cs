using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageOverTimeInAnAreaSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] DamageInfo _damage;
    [SerializeField] float _damageCooldown;
    [SerializeField] float _damageAreaRadius;
    [SerializeField] LayerMask _affectedLayers;
    [SerializeField] ParticleSystem _particles;

    Dictionary<GameObject, float> _damagedObjsCooldownTimers = new Dictionary<GameObject, float>();
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var dealDamageOverTimeOriginal = original as DealDamageOverTimeInAnAreaSupportObjBehaviour;
        _damage = dealDamageOverTimeOriginal._damage;
        _damageCooldown = dealDamageOverTimeOriginal._damageCooldown;
        _damageAreaRadius = dealDamageOverTimeOriginal._damageAreaRadius;
        _affectedLayers = dealDamageOverTimeOriginal._affectedLayers;
        _particles = dealDamageOverTimeOriginal._particles;
        OnStart += GenerateParticles;
        OnUpdate += ReduceCooldowns;
        OnUpdate += CheckArea;
    }

    void GenerateParticles()
    {
        ParticleConfig particleConfig = new(_particles, ObjControl.transform.position, Quaternion.identity, -1);
        var particlesObj = ParticleManager.pm.SpawnParticles(particleConfig);
        particlesObj.transform.SetParent(ObjControl.transform, true);
    }

    void ReduceCooldowns()
    {
        foreach(var key in _damagedObjsCooldownTimers.Keys)
        {
            if (_damagedObjsCooldownTimers[key] > 0)
                _damagedObjsCooldownTimers[key] -= Time.deltaTime;
        }
    }

    void CheckArea()
    {
        var affectedObjs = Physics2D.OverlapCircleAll(ObjControl.transform.position, _damageAreaRadius, _affectedLayers);
        foreach (var obj in affectedObjs)
        {
            var objHP = obj.GetComponent<HP>();
            if (objHP == null)
                continue;

            GameObject damagedObj = obj.transform.root.gameObject;
            if (_damagedObjsCooldownTimers.ContainsKey(damagedObj) && _damagedObjsCooldownTimers[damagedObj] > 0)
                continue;
            
            objHP.TakeDamage((int)_damage.CalculatedDamage);

            if (_damagedObjsCooldownTimers.ContainsKey(damagedObj))
                _damagedObjsCooldownTimers[damagedObj] = _damageCooldown;
            else
                _damagedObjsCooldownTimers.Add(damagedObj, _damageCooldown);
        }

    }
}
