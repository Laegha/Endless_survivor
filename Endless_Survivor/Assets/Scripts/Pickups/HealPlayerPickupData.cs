using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Pickup", menuName = "ScriptableObjects/Pickups/Heal", order = 4)]
public class HealPlayerPickupData : PickupData
{
    [SerializeField] CustomAnimation _pickupAnimation;
    [SerializeField] int _healAmmount;
    public override void TransferData(PickupControl pickupControl)
    {
        base.TransferData(pickupControl);
        pickupControl.Animator.AddAnimations(new List<CustomAnimation> { _pickupAnimation });
        pickupControl.Animator.ChangeAnim(_pickupAnimation);
    }
    public override void PickUp(PickupControl pickupControl)
    {
        base.PickUp(pickupControl);
        PlayerControl.pc.PlayerHPManager.Heal(_healAmmount);
    }
}