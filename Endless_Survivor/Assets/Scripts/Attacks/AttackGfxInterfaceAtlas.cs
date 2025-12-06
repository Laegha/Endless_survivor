using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGfxInterfaceAtlas
{
    [SerializeField] AnimationChangeAttackGfxInterface animChangeInterface;
    AttackGfxInterface[] _interfaces => new AttackGfxInterface[] { animChangeInterface };
    public void ApplyGfxToAttack(Attack attack)
    {
        var interfaces = _interfaces;
        foreach (var gfxInterface in interfaces)
        {
            if(gfxInterface.GetType() != attack.AttackGfxInterface.GetType())
                continue;
            attack.ChangeGfx(gfxInterface);
        }
    }
}
