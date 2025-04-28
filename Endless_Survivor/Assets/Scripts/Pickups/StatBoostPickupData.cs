using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StatBoost Pickup", menuName = "ScriptableObjects/Pickups/StatBoost", order = 1)]
public class StatBoostPickupData : PickupData
{
    [SerializeField] CustomAnimation _pickupAnimation;
    [SerializeField] PlayerStats _statsBoost;
    public override void TransferData(PickupControl pickupControl)
    {
        base.TransferData(pickupControl);
        CustomAnimator pickupAnimator = pickupControl.GetComponent<PickupControl>().Animator;
        pickupAnimator.AddAnimations(new List<CustomAnimation> { _pickupAnimation });
        pickupAnimator.ChangeAnim(_pickupAnimation.AnimationName);
        pickupControl.gameObject.AddComponent<StatBoost>();

    }
}
