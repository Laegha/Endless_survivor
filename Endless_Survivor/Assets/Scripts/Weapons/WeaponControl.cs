using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    [SerializeField] WeaponAim _weaponAim;
    [SerializeField] WeaponAttackManager _weaponAttackManager;
    [SerializeField] CustomAnimator _weaponAnimator;
    [SerializeField] SpriteRenderer _gfx;
    [SerializeField] RendererSortingByY _rendererSorter;
    [SerializeField] ParticleSystem _onLevelUpParticle;
    public WeaponAim WeaponAim { get { return _weaponAim; } }
    public WeaponAttackManager WeaponAttackManager { get { return _weaponAttackManager; } }
    public CustomAnimator WeaponAnimator { get { return _weaponAnimator; } }
    public SpriteRenderer Gfx { get { return _gfx; } }
    public RendererSortingByY GfxSorter { get { return _rendererSorter; } }
    public ParticleSystem OnLevelUpParticle { get { return _onLevelUpParticle; } }
}
