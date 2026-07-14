using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : HP
{
    float _knockbackResistance;

    List<RouletteElementChance<PickupData>> _dropablePickupsChances;
    [SerializeField] float _damagedFlashingTime = 1.5f;
    [SerializeField] EnemyControl _enemyControl;
    float _damagedFlashingTimer;
    [SerializeField] float _damagedFlashingRate = .1f;
    [SerializeField] Material _damagedFlashingMaterial;
    static readonly int _flashingMaterialAuthority = 5;
    SpriteMaterialFlashing _damagedFlashing;
    Action<EnemyControl> _onDeath;

    SFXInfo _onHitSound;
    SFXInfo _onDeathSound;
    public Action<EnemyControl> OnDeath { get { return _onDeath; } set { _onDeath = value; } }
    public float KnockbackResistance { set { _knockbackResistance = value; } }  
    public List<RouletteElementChance<PickupData>> DropablePickupChances { set { _dropablePickupsChances = value; } }
    private void Start()
    {
        _damagedFlashing = new SpriteMaterialFlashing(_enemyControl.MaterialManager, _damagedFlashingRate, new MaterialOverride(_flashingMaterialAuthority, _damagedFlashingMaterial));
    }
    public void SetSounds(SFXInfo onHitSound, SFXInfo onDeathSound)
    {
        _onHitSound = onHitSound;
        _onDeathSound = onDeathSound;
    }
    public override void TakeDamage(int incomingDamage)
    {
        base.TakeDamage(incomingDamage);
        SoundFXManager.sm.PlaySfx(_onHitSound, transform.position);
        if(_damagedFlashingTimer <= 0)
            _damagedFlashing?.Start();
        _damagedFlashingTimer = _damagedFlashingTime;
        RunStatsManager.runStatsManager.DamageDealt(incomingDamage);
    }
    public void TakeDamage(int incomingDamage, Vector2 impactDirection, float knockbackPower)
    {
        var knockbackForce = KnockbackUtility.GetKnockbackForceInfo(impactDirection, Mathf.Clamp(knockbackPower - _knockbackResistance,0, Mathf.Infinity));//Maybe there sould be a max so the enemies don't fly off the map
        _enemyControl.RbForcesController.ChangeCurrForce(knockbackForce);
        TakeDamage(incomingDamage);
    }
    private void Update()
    {
        if (_damagedFlashingTimer <= 0)
            return;
        _damagedFlashing.Update();
        _damagedFlashingTimer -= Time.deltaTime;
        if(_damagedFlashingTimer <= 0)
        {
            _damagedFlashing.End();
        }
    }
    public override void Die()
    {
        _onDeath?.Invoke(_enemyControl);
        InstantiatePickup(); 
        SoundFXManager.sm.PlaySfx(_onDeathSound, transform.position);
        RunStatsManager.runStatsManager.EnemyKilled(_enemyControl);
        Destroy(gameObject);
    }
    void InstantiatePickup()
    {
        var resultPickup = Utility.GetRouletteElementWithNullChance(_dropablePickupsChances);
        if (resultPickup == null)
            return;
        Utility.GeneratePickup(resultPickup, transform.position);
    }
}