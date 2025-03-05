using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    [SerializeField] CustomAnimator _weaponAnimator;
    public CustomAnimator WeaponAnimator { get { return _weaponAnimator; } }
}
