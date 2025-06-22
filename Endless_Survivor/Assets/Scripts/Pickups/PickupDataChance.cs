using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PickupDataChance
{
    [SerializeField] PickupData _pickupData;
    [SerializeField] int _chance;
    public PickupData PickupData {  get { return _pickupData; } }
    public int Chance { get { return _chance; } set { _chance = value; } }
}
