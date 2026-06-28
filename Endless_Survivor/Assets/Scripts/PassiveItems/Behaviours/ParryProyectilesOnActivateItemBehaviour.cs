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
    [SerializeField] SFXInfo _onParrySFX;
    CustomAnimator _parryAnimator;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var parryOnActivateOriginal = original as ParryProyectilesOnActivateItemBehaviour;
        _parryRadius = parryOnActivateOriginal._parryRadius;
        _parryAnimation = parryOnActivateOriginal._parryAnimation;
        _parriedProyectileAnimation = parryOnActivateOriginal._parriedProyectileAnimation;
        _onParrySFX = parryOnActivateOriginal._onParrySFX;
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
            proyectile.GetParried(_parriedProyectileAnimation);
        }
        SoundFXManager.sm.PlaySfx(_onParrySFX, PlayerControl.pc.transform.position);
    }
    public override void RemoveBehaviour()
    {
        if (_parryAnimator != null)
            GameObject.Destroy(_parryAnimator.gameObject);
    }
}
