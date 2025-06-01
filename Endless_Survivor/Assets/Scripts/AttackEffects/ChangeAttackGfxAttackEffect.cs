using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAttackGfxAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    //List<AttackGfxHandler> attackGfxHandlers = new();
    [SerializeReference]List<AttackGfxInterface> _gfxInterfaces = new();
    public List<AttackGfxInterface> GfxInterfaces { get { return _gfxInterfaces; } }
    public ChangeAttackGfxAttackEffect(AttackEffect original, Attack affectedAttack) : base (original, affectedAttack) 
    {
        base.OnAttack += ChangeAtkGfx;
    }
    public void ChangeAtkGfx()
    {
        //AttackGfxInterface attackGfxInterface = _gfxInterfaces.Find(x => x.weaponType == attackingWeapon.GetType());
        //attackingWeapon?.ChangeAttackGfx(attackGfxInterface);
    }
}
