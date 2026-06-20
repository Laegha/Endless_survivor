using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupData : ScriptableObject
{
    [SerializeField] ParticleSystem _pickupParticles;
    [SerializeField] Vector2 _colliderSize;
    [SerializeField] int _renderingOffset;
    public virtual void TransferData(PickupControl pickupControl)
    {
        pickupControl.Pickup.PickupData = this;
        if(_pickupParticles != null)
        {
            var particleConfig = new ParticleConfig(_pickupParticles, Vector2.zero, Quaternion.identity, -1, pickupControl.transform, true, true);
            ParticleManager.pm.SpawnParticles(particleConfig);

        }
        pickupControl.RendererSorting.Offset = _renderingOffset;
        pickupControl.Collider.size = _colliderSize;
    }
    public virtual void PickUp(PickupControl pickupControl)
    {
        Destroy(pickupControl.gameObject);
    }
}
