using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSetter : MonoBehaviour
{
    [SerializeField] List<CustomAnimation> _animations;
    [SerializeField] CustomAnimator _animator;

    private void Start()
    {
        _animator.AddAnimations(_animations);
    }
}
