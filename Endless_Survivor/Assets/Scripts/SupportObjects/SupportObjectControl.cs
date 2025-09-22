using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportObjectControl : MonoBehaviour
{
    [SerializeField] SupportObjectBehaviourManager _behaviourManager;
    [SerializeField] Transform _colliderHolder;
    [SerializeField] CustomAnimator _animator;

    public SupportObjectBehaviourManager BehaviourManager { get { return _behaviourManager; } }
    public Transform ColliderHolder { get { return _colliderHolder; } }
    public CustomAnimator Animator { get { return _animator; } }
}
