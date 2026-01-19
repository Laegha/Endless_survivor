using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipRendererByMoveDirEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] bool _spriteFacingRight;
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var flipRendererByDir = original as FlipRendererByMoveDirEnemyBehaviour;
        _spriteFacingRight = flipRendererByDir._spriteFacingRight;
    }
    public override void PassiveUpdate()
    {
        base.PassiveUpdate();
        Vector2 direction = EnemyControl.RbForcesController.Rb.velocity.normalized;
        if (direction == Vector2.zero)
            return;
        bool rbFacingRight = direction.x > 0;
        bool result = _spriteFacingRight != rbFacingRight;
        foreach (var renderer in EnemyControl.Renderers)
        {
            renderer.flipX = result;
        }
    }

}
