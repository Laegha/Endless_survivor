using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] CustomAnimator _pickupAnimator;
    public CustomAnimator PickupAnimator { get { return _pickupAnimator; } }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickUp(collision.GetComponent<PlayerControl>());
    }

    public virtual void PickUp(PlayerControl playerControl) { }
}
