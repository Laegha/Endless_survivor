using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageSource : DamageSource
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Player"))
            return;
        PlayerControl.pc.PlayerHPManager.TakeDamage(Damage);
    }
}
