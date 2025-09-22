using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportObjectCollisionDetector : MonoBehaviour
{
    SupportObjectBehaviourManager _behaviourManager;
    public SupportObjectBehaviourManager BehaviourManager { set { _behaviourManager = value; } }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(PlayerControl.pc.transform == collision.transform.root)
        {
            _behaviourManager.CollidedWithPlayer();
            return;
        }
        var collidedEnemy = collision.GetComponent<EnemyControl>();
        if (collidedEnemy != null)
            _behaviourManager.CollidedWithEnemy(collidedEnemy);

    }
}
