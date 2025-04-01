using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupControl : MonoBehaviour
{
    [SerializeField] CustomAnimator _animator;
    public CustomAnimator Animator { get { return _animator; } }
}
