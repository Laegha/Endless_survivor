using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Change Condition Points Pickup", menuName = "ScriptableObjects/Pickups/ChangeAttackConditionPoints", order = 7)]
public class ChangeAttackConditionPointsPickupData : PickupData
{
    [SerializeField] CustomAnimation _pickupAnimation;
    [SerializeField] int _points;
    public static readonly string pickupWeaponId = "weapon";
    static readonly string _pointsId = "points";


    public override void TransferData(PickupControl pickupControl)
    {
        base.TransferData(pickupControl);
        pickupControl.Pickup.AddVariable<int>(_pointsId, _points);
        pickupControl.Animator.AddAnimations(new() { _pickupAnimation });

    }
    public override void PickUp(PickupControl pickupControl)
    {
        base.PickUp(pickupControl);
        WeaponAttackManager relatedWeapon = pickupControl.Pickup.GetVariable<WeaponAttackManager>(pickupWeaponId);
        if (!PointBasedChangeCondition.WeaponPoints.ContainsKey(relatedWeapon))
            return;
        int points = pickupControl.Pickup.GetVariable<int>(_pointsId);
        PointBasedChangeCondition.WeaponPoints[relatedWeapon] = _points;
    }
}
