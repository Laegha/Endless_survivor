using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : HP
{
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
    public List<PickupDataChance> DropablePickupChances { set { _dropablePickupsChances = value; } }
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
        _enemyControl.StatusEffectManager.OnHit();
        SoundFXManager.sm.PlaySfx(_onHitSound, transform.position);
        print(RemainingHP);
        _damagedFlashing.Start();
        _damagedFlashingTimer = _damagedFlashingTime;
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
        Destroy(gameObject);
    }
    void InstantiatePickup()
    {
        Dictionary<PickupData, int> possiblePickups = new Dictionary<PickupData, int>();
        possiblePickups.Add(null, 100);
        foreach (var dropablePickupChance in _dropablePickupsChances)
        {
            possiblePickups[null] = Mathf.Clamp(possiblePickups[null] - dropablePickupChance.Chance, 0, 100);
            possiblePickups.Add(dropablePickupChance.PickupData, dropablePickupChance.Chance);
            print("Added " + dropablePickupChance.PickupData + " to possible pickups with a chance of " +  dropablePickupChance.Chance);
        }
        Roulette<PickupData> pickupRoulette = new Roulette<PickupData>(possiblePickups);
        PickupData resultPickup = pickupRoulette.Spin();
        if (resultPickup == null)
            return;
        GameObject newPickup = Instantiate(GameManager.gm.prefabHolder.Prefabs["Pickup"], transform.position, Quaternion.identity);
        resultPickup.TransferData(newPickup.GetComponent<PickupControl>());
    }
}