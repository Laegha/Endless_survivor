using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponizeSupportObjBehaviour : SupportObjectBehaviour
{
    [SerializeField] WeaponData _objWeapon;
    [SerializeField] bool _setSupportObjAsDistCheck;
    WeaponAttackManager _weaponAttackManager;
    new public static int maxStacks => 1;
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var weaponizeOriginal = original as WeaponizeSupportObjBehaviour;
        _objWeapon = weaponizeOriginal._objWeapon;
        _setSupportObjAsDistCheck = weaponizeOriginal._setSupportObjAsDistCheck;

        OnStart += CreateWeapon;
        WaveManager.wm.OnWaveStarted += UpdateWeaponStats;
        
    }
    void CreateWeapon()
    {
        GameObject theoricHand = new("TheoricHand");
        theoricHand.transform.SetParent(ObjControl.transform, false);
        //theoricHand.transform.position = Vector2.zero;

        var weaponObj = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["Weapon"], theoricHand.transform);
        weaponObj.transform.position = Vector2.zero;
        weaponObj.transform.rotation = Quaternion.identity;

        var weaponStats = new WeaponStats(_objWeapon.WeaponStats);
        weaponStats.SetTrueLevelStats(_objWeapon.StatsIncreaseScale, ScalingFunctions.CurrWaveLevel);

        var weaponControl = weaponObj.GetComponent<WeaponControl>();
        _weaponAttackManager = weaponControl.WeaponAttackManager;
        _weaponAttackManager.Initiate(_objWeapon.AttackConditions, weaponStats, _objWeapon);

        if(_setSupportObjAsDistCheck)
            weaponControl.WeaponAim.ChangeDistCheckPos(() => ObjControl.transform.position);

        foreach (var renderer in ObjControl.Renderers)
        {
            renderer.enabled = false;
        }
        ObjControl.Renderers.AddRange(weaponControl.GfxSorter.AffectedRenderers);
        var objAnimations = ObjControl.Animator.Animations;
        ObjControl.Animator = weaponControl.WeaponAnimator;
        ObjControl.Animator.AddAnimations(objAnimations);
    }
    void UpdateWeaponStats()
    {
        var weaponStats = new WeaponStats(_objWeapon.WeaponStats);
        weaponStats.SetTrueLevelStats(_objWeapon.StatsIncreaseScale, ScalingFunctions.CurrWaveLevel);
        _weaponAttackManager.WeaponStats = weaponStats;
    }
}
