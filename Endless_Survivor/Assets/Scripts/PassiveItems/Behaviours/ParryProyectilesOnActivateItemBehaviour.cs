using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ParryProyectilesOnActivateItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] float _parryRadius;
    [SerializeField] CustomAnimation _parryAnimation;
    [SerializeField] CustomAnimation _parriedProyectileAnimation;
    CustomAnimator _parryAnimator;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var parryOnActivateOriginal = original as ParryProyectilesOnActivateItemBehaviour;
        _parryRadius = parryOnActivateOriginal._parryRadius;
        _parryAnimation = parryOnActivateOriginal._parryAnimation;
        _parriedProyectileAnimation = parryOnActivateOriginal._parriedProyectileAnimation;
    }
    public override void Activate()
    {
        base.Activate();
        //do animated obj on player
        if (_parryAnimator == null && _parryAnimation != null && _parryAnimation.Frames.Length != 0)
        {
            AnimatedObjConfig parryAnim = new(_parryAnimation, Vector2.zero, Quaternion.identity, _parryAnimation.AnimDuration, PlayerControl.pc.transform);
            _parryAnimator = AnimatedObjsManager.aom.SpawnAnimatedObj(parryAnim);

        }

        var objsInParryRange = Physics2D.OverlapCircleAll(PlayerControl.pc.transform.position, _parryRadius).ToList();
        List<EnemyProyectile> parriedProyectiles = objsInParryRange.Where(x => x.GetComponent<EnemyProyectile>() != null).Select(x => x.GetComponent<EnemyProyectile>()).ToList();

        foreach(var proyectile in parriedProyectiles)
        {
            if (proyectile.GetComponent<EnemyDamageSource>() != null)
                continue;
            proyectile.GetComponent<Rigidbody2D>().velocity *= -1;
            int proyectileDamage = proyectile.GetComponent<PlayerDamageSource>().Damage;
            proyectile.AddComponent<EnemyDamageSource>().Damage = proyectileDamage;
            //add animated obj in proyectile
            if(_parriedProyectileAnimation != null && _parriedProyectileAnimation.Frames.Length > 0)
            {
                AnimatedObjConfig proyectileAnimConfig = new(_parriedProyectileAnimation, Vector2.zero, Quaternion.identity, -1, proyectile.transform);
                var animatedObj = AnimatedObjsManager.aom.SpawnAnimatedObj(proyectileAnimConfig);
                animatedObj.GetComponent<RendererSortingByY>().Offset = 5;
            }
            proyectile.gameObject.layer = LayerMask.NameToLayer("PlayerAttack");
        }


    }
}
