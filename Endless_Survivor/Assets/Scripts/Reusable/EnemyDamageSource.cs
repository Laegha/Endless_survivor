using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageSource : DamageSource
{
    public override void OnTriggerEnter2D(Collider2D collider)
    {
        if (IgnoreColliders.Contains(collider))
            return;
        EnemyControl hitEnemyControl = collider.transform.root.GetComponent<EnemyControl>();
        if (hitEnemyControl != null)
            hitEnemyControl.EnemyHP.TakeDamage(Damage);
    }
}
