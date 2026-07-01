using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] 
public class WeaponHolderInfo
{
    [SerializeField]Sprite[] _possibleHandSprites;
    [SerializeField]bool _permanent;
    [SerializeField]bool _useFixedPosition;
    [SerializeField]Vector2 _fixedPosition;
    public Sprite RandomHandSprite { get { return _possibleHandSprites[UnityEngine.Random.Range(0, _possibleHandSprites.Length)]; } }
    public bool Permanent { get { return _permanent; } }
    public bool UseFixedPosition { get { return _useFixedPosition; } }
    public Vector2 FixedPosition { get { return _fixedPosition; } }
}
