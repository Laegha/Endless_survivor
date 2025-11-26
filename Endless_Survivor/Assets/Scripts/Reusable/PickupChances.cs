using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PickupChances
{
    [SerializeField] List<RouletteElementChance<PickupData>> _dropablePickupChances = new List<RouletteElementChance<PickupData>>();
    public List<RouletteElementChance<PickupData>> DropablePickupChances { get {  return _dropablePickupChances; } }
}
