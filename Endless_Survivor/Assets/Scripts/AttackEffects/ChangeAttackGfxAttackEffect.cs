using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ChangeAtkGfx", menuName = "ScriptableObjects/AttackEffects/ChangeAtkGfx", order = 0)]
public class ChangeAttackGfxAttackEffect : AttackEffect
{
    //List<AttackGfxHandler> attackGfxHandlers = new();
    [SerializeReference]List<AttackGfxInterface> _gfxInterfaces = new();
    public List<AttackGfxInterface> GfxInterfaces { get { return _gfxInterfaces; } }
    public override void OnAttack(Weapon attackingWeapon)
    {
        base.OnAttack(attackingWeapon);
        AttackGfxInterface attackGfxInterface = _gfxInterfaces.Find(x => x.weaponType == attackingWeapon.GetType());
        attackingWeapon?.ChangeAttackGfx(attackGfxInterface);
    }
}
