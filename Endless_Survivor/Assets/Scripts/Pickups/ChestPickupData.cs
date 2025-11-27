using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Chest Pickup", menuName = "ScriptableObjects/Pickups/Chest", order = 3)]
public class ChestPickupData : PickupData
{
    [Header("Possible chest drops")]
    [SerializeReference] PickupChances _pickupChances = new();
    [SerializeField] SupportObjectData _generatedObjIfNullDrop;
    [SerializeField] Vector2 _dropOffset;
    [SerializeField] CustomAnimation _idleAnimation;
    [SerializeField] CustomAnimation _openAnimation;
    [SerializeField] int _dropFrame;
    public override void TransferData(PickupControl pickupControl)
    {
        base.TransferData(pickupControl);
        CustomAnimation openAnim = new CustomAnimation(pickupControl.Animator, _openAnimation);
        AnimationEvent dropPickupEvent = new(null, _dropFrame, () => DropPickup((Vector2)pickupControl.transform.position + _dropOffset));
        openAnim.Events.Add(dropPickupEvent);
        pickupControl.Animator.AddAnimations(new List<CustomAnimation> { _idleAnimation, openAnim});
        pickupControl.Animator.ChangeAnim(_idleAnimation.AnimationName);
    }
    public override void PickUp(PickupControl pickupControl)
    {
        pickupControl.Animator.ChangeAnim(_openAnimation.AnimationName);
        Destroy(pickupControl.gameObject, _openAnimation.AnimDuration);
    }
    void DropPickup(Vector2 position)
    {
        var dropedPickup = Utility.GetRouletteElementWithNullChance(_pickupChances.DropablePickupChances);
        if (dropedPickup != null)
        {
            Utility.GeneratePickup(dropedPickup, position);
            Debug.Log("Generationg " + dropedPickup);
            return;
        }
        if(_generatedObjIfNullDrop == null)
            return;
        var supportObj = Instantiate(GameManager.gm.prefabHolder.Prefabs["SupportObject"], position, Quaternion.identity).GetComponent<SupportObjectControl>();    
        _generatedObjIfNullDrop.TransferData(supportObj);
    }
}