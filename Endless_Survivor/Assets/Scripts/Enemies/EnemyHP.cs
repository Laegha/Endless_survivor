using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    int _leftHP;
    Dictionary<PickupData, int> _dropablePickupsChances;
    public int LeftHP {  get { return _leftHP; } set { _leftHP = value; } }
    public Dictionary<PickupData, int> DropablePickupChances { set {  _dropablePickupsChances = value; } }

    public void RecieveDamage(int incomingDamage)
    {
        _leftHP -= incomingDamage;
        if (_leftHP < 0)
            Die();

    }

    void Die()
    {
        InstantiatePickup();
        WaveManager.wm.EnemyKilled(gameObject);
    }
    void InstantiatePickup()
    {
        Dictionary<dynamic, int> possiblePickups = new Dictionary<dynamic, int>();
        possiblePickups.Add(null, 100);
        foreach(var dropablePickupChance in _dropablePickupsChances)
        {
            possiblePickups[null] = Mathf.Clamp(possiblePickups[null] - dropablePickupChance.Value, 0, 100);
            possiblePickups.Add(dropablePickupChance.Key, dropablePickupChance.Value);
        }
        Roulette pickupRoulette = new Roulette(possiblePickups);
        PickupData resultPickup = pickupRoulette.Spin();
        if (resultPickup == null)
            return;
        GameObject newPickup = Instantiate(GameManager.gm.prefabHolder.Prefabs["Pickup"], transform.position, Quaternion.identity);
        resultPickup.TransferData(newPickup);
    }
}
