using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : HP
{
    List<PickupDataChance> _dropablePickupsChances;
    [SerializeField] float _damagedFlashingTime = 1.5f;
    float _damagedFlashingTimer;
    [SerializeField] float _damagedFlashingRate = .1f;
    [SerializeField] Material _defaultMaterial;
    [SerializeField] Material _damagedFlashingMaterial;
    [SerializeField] SpriteRenderer[] _renderers;
    SpriteMaterialFlashing _damagedFlashing;

    SFXInfo _onHitSound;
    SFXInfo _onDeathSound;
    public List<PickupDataChance> DropablePickupChances { set { _dropablePickupsChances = value; } }
    private void Start()
    {
        _damagedFlashing = new SpriteMaterialFlashing(_renderers, _damagedFlashingRate, _defaultMaterial, _damagedFlashingMaterial);
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
        InstantiatePickup();
        SoundFXManager.sm.PlaySfx(_onDeathSound, transform.position);
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
            print("Added " + dropablePickupChance.PickupData + " to possible pickups with a chance of " +  dropablePickupChance.Chance);
        }
        Roulette<PickupDataKey> pickupRoulette = new Roulette<PickupDataKey>(possiblePickups);
        PickupDataKey resultPickup = pickupRoulette.Spin();
        if (resultPickup.pickupData == null)
            return;
        GameObject newPickup = Instantiate(GameManager.gm.prefabHolder.Prefabs["pickup"], transform.position, Quaternion.identity);
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