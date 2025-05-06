using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupData : ScriptableObject
{
    public virtual void TransferData(PickupControl pickupControl)
    {
        pickupControl.Pickup.PickupData = this;
    }
    public virtual void PickUp(PickupControl pickupControl) { }
}
