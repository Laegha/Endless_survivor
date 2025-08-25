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

    Action<EnemyControl> _onEnemyDeath;
    Action<EnemyControl, int> _onEnemyDamaged;

    SFXInfo _onHitSound;
    SFXInfo _onDeathSound;
    public List<PickupDataChance> DropablePickupChances { set { _dropablePickupsChances = value; } }
    public Action<EnemyControl> OnEnemyDeath { get { return _onEnemyDeath; } set { _onEnemyDeath = value; } }
    public Action <EnemyControl, int> OnEnemyDamaged { get { return _onEnemyDamaged; } set  { _onEnemyDamaged = value; } }
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
        _damagedFlashing?.Start();
        _damagedFlashingTimer = _damagedFlashingTime;
        _onEnemyDamaged?.Invoke(_enemyControl, incomingDamage);
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
        _onEnemyDeath?.Invoke(_enemyControl);
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