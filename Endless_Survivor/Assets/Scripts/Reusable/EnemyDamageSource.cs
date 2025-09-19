using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageSource : DamageSource
{
    [SerializeField] float _knockback;
    public float Knockback { set { _knockback = value; } }
    public override void OnTriggerEnter2D(Collider2D collider)
    {
        if (IgnoreColliders.Contains(collider))
            return;
        EnemyControl hitEnemyControl = collider.transform.root.GetComponent<EnemyControl>();
        if (hitEnemyControl != null)
        {
            Vector2 impactDirection = hitEnemyControl.transform.position - transform.position;
            hitEnemyControl.EnemyHP.TakeDamage(Damage, impactDirection, _knockback);
        }
    }
}
