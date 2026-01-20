using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Change Condition Points Pickup", menuName = "ScriptableObjects/Pickups/ChangeAttackConditionPoints", order = 7)]
public class ChangeAttackConditionPointsPickupData : PickupData
{
    [SerializeField] CustomAnimation _pickupAnimation;
    [SerializeField] int _points;
    [SerializeField] string _pickupWeaponId;
    static readonly string _pointsId = "points";

    public string PickupWeaponId { get { return _pointsId; } }

    public override void TransferData(PickupControl pickupControl)
    {
        base.TransferData(pickupControl);
        pickupControl.Pickup.AddVariable<int>(_pointsId, _points);

    }
    public override void PickUp(PickupControl pickupControl)
    {
        base.PickUp(pickupControl);
        WeaponAttackManager relatedWeapon = pickupControl.Pickup.GetVariable<WeaponAttackManager>(_pickupWeaponId);
        int points = pickupControl.Pickup.GetVariable<int>(_pointsId);
        PointBasedChangeCondition.WeaponPoints[relatedWeapon] = _points;
    }
}
