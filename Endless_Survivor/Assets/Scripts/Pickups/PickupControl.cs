using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupControl : MonoBehaviour
{
    [SerializeField] CustomAnimator _animator;
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] Pickup _pickup;
    public CustomAnimator Animator { get { return _animator; } }
    public SpriteRenderer Renderer { get { return _renderer; } }
    public Pickup Pickup { get { return _pickup; } }
}
