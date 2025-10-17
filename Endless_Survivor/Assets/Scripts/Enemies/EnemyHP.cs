using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : HP
{
    float _knockbackResistance;

    List<PickupDataChance> _dropablePickupsChances;
    [SerializeField] float _damagedFlashingTime = 1.5f;
    [SerializeField] EnemyControl _enemyControl;
    float _damagedFlashingTimer;
    [SerializeField] float _damagedFlashingRate = .1f;
    [SerializeField] Material _damagedFlashingMaterial;
    static readonly int _flashingMaterialAuthority = 5;
    SpriteMaterialFlashing _damagedFlashing;

    SFXInfo _onHitSound;
    SFXInfo _onDeathSound;
    public float KnockbackResistance { set { _knockbackResistance = value; } }  
    public List<PickupDataChance> DropablePickupChances { set { _dropablePickupsChances = value; } }
    private void Start()
    {
        _damagedFlashing = new SpriteMaterialFlashing(_enemyControl.MaterialManager, _damagedFlashingRate, new MaterialOverride(_flashingMaterialAuthority, _damagedFlashingMaterial));
        OnDamageTaken += _enemyControl.StatusEffectManager.OnHit;
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
        _enemyControl.StatusEffectManager.OnKilled();
        WaveManager.wm.EnemyKilled(gameObject);
        InstantiatePickup(); 
        SoundFXManager.sm.PlaySfx(_onDeathSound, transform.position);
        RunStatsManager.runStatsManager.EnemyKilled(_enemyControl);
        Destroy(gameObject);
    }
    void InstantiatePickup()
    {
        Dictionary<PickupDataKey, int> possiblePickups = new Dictionary<PickupDataKey, int>();
        PickupDataKey nullPickupData = new PickupDataKey(null);
        possiblePickups.Add(nullPickupData, 100);
        foreach (var dropablePickupChance in _dropablePickupsChances)
        {
            possiblePickups[nullPickupData] = Mathf.Clamp(possiblePickups[nullPickupData] - dropablePickupChance.Chance, 0, 100);
            possiblePickups.Add(new PickupDataKey(dropablePickupChance.PickupData), dropablePickupChance.Chance);
        }
        Roulette<PickupDataKey> pickupRoulette = new Roulette<PickupDataKey>(possiblePickups);
        PickupDataKey resultPickup = pickupRoulette.Spin();
        if (resultPickup.pickupData == null)
            return;
        GameObject newPickup = Instantiate(GameManager.gm.prefabHolder.Prefabs["Pickup"], transform.position, Quaternion.identity);
        resultPickup.pickupData.TransferData(newPickup.GetComponent<PickupControl>());
    }
}

class PickupDataKey
{
    public PickupData pickupData;
    public PickupDataKey(PickupData data)
    {
        this.pickupData = data;
    }
}