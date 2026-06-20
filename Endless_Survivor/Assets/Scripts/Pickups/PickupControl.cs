using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupControl : MonoBehaviour
{
    [SerializeField] CustomAnimator _animator;
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] RendererSortingByY _rendererSorting;
    [SerializeField] Pickup _pickup;
    [SerializeField] BoxCollider2D _collider;
    public CustomAnimator Animator { get { return _animator; } }
    public SpriteRenderer Renderer { get { return _renderer; } }
    public RendererSortingByY RendererSorting { get { return _rendererSorting; } }
    public Pickup Pickup { get { return _pickup; } }
    public BoxCollider2D Collider { get { return _collider; } }
}
