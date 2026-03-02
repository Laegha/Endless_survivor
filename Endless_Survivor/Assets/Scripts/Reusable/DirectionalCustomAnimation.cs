using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class DirectionalCustomAnimation
{
    [SerializeField] CustomAnimation _upAnimation;
    [SerializeField] CustomAnimation _rightAnimation;
    [SerializeField] CustomAnimation _downAnimation;
    [SerializeField] CustomAnimation _leftAnimation;
    List<CustomAnimation> _nonNullAnimations = new List<CustomAnimation>();
    public List<CustomAnimation> NonNullAnimations { get { return _nonNullAnimations; } }

    public DirectionalCustomAnimation(CustomAnimator animator, DirectionalCustomAnimation original)
    {
        _upAnimation = new(animator, original._upAnimation);
        _rightAnimation = new(animator, original._rightAnimation);
        _downAnimation = new(animator, original._downAnimation);
        _leftAnimation = new(animator, original._leftAnimation);

        List<CustomAnimation> anims = new();
        if (_upAnimation != null && _upAnimation.Frames.Length > 0)
            _nonNullAnimations.Add(_upAnimation);

        if (_rightAnimation != null && _rightAnimation.Frames.Length > 0)
            _nonNullAnimations.Add(_rightAnimation);

        if (_leftAnimation != null && _leftAnimation.Frames.Length > 0)
            _nonNullAnimations.Add(_leftAnimation);

        if (_downAnimation != null && _downAnimation.Frames.Length > 0)
            _nonNullAnimations.Add(_downAnimation);
    }
    public CustomAnimation GetAnim(Vector2 dir)
    {
        return Utility.GetAnimFromDirection(dir, _upAnimation, _rightAnimation, _downAnimation, _leftAnimation);
    }
}
