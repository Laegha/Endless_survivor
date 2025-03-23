using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    [SerializeField] CustomAnimator _weaponAnimator;
    [SerializeField] SpriteRenderer _gfx;
    public CustomAnimator WeaponAnimator { get { return _weaponAnimator; } }
    public SpriteRenderer Gfx { get { return _gfx; } }
}
