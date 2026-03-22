using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportObjectControl : MonoBehaviour
{
    [SerializeField] SupportObjectBehaviourManager _behaviourManager;
    [SerializeField] Transform _colliderHolder;
    [SerializeField] CustomAnimator _animator;
    [SerializeField] List<SpriteRenderer> _renderers;
    [SerializeField] RendererSortingByY _rendererSorter;

    public SupportObjectBehaviourManager BehaviourManager { get { return _behaviourManager; } }
    public Transform ColliderHolder { get { return _colliderHolder; } }
    public CustomAnimator Animator { get { return _animator; } set { _animator = value; } }
    public List<SpriteRenderer> Renderers { get { return _renderers; } }
    public RendererSortingByY RendererSorter { get { return _rendererSorter; } }
}
