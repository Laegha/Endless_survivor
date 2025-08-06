using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    [SerializeField] CustomAnimator _weaponAnimator;
    [SerializeField] SpriteRenderer _gfx;
    [SerializeField] ParticleSystem _onLevelUpParticle;
    public CustomAnimator WeaponAnimator { get { return _weaponAnimator; } }
    public SpriteRenderer Gfx { get { return _gfx; } }
    public ParticleSystem OnLevelUpParticle { get { return _onLevelUpParticle; } }
}
