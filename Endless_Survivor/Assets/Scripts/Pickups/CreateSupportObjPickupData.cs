using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new SupportObj Pickup", menuName = "ScriptableObjects/Pickups/Create Support Obj Pickup", order = 6)]
public class CreateSupportObjPickupData : PickupData
{
    [SerializeField] CustomAnimation _pickupAnim;
    [SerializeField] SupportObjectData _createdSupportObj;
    public override void TransferData(PickupControl pickupControl)
    {
        base.TransferData(pickupControl);
        pickupControl.Animator.AddAnimations(new List<CustomAnimation> { _pickupAnim });
        pickupControl.Animator.ChangeAnim(_pickupAnim.AnimationName);
    }
    public override void PickUp(PickupControl pickupControl)
    {
        base.PickUp(pickupControl);
        Utility.GenerateSupportObj(_createdSupportObj, pickupControl.transform.position, Quaternion.identity);
    }
}
