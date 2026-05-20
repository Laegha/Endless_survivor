using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackOnActivateItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] MeleeData _meleeData;
    [SerializeField] DamageInfo _damage;
    [SerializeField] float _knockBack;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var meleeAttackOriginal = original as MeleeAttackOnActivateItemBehaviour;
        _meleeData = meleeAttackOriginal._meleeData;
        _damage = meleeAttackOriginal._damage;
        _knockBack = meleeAttackOriginal._knockBack;
    }

    public override void Activate()
    {
        base.Activate();
        
        MeleeAttack meleeAttack = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["Melee"], PlayerControl.pc.transform.position, Quaternion.identity).GetComponent<MeleeAttack>();
        meleeAttack.TriggersPassiveItemHit = false;
        meleeAttack.transform.SetParent(PlayerControl.pc.transform, true);
        meleeAttack.StartAttack((int)_damage.CalculatedDamage, _knockBack, _meleeData);
        meleeAttack.ApplyDamage();
        GameObject.Destroy(meleeAttack.gameObject, _meleeData.AttackVfxAnimation.AnimDuration);
    }
    public override void RemoveBehaviour()
    {

    }
}
