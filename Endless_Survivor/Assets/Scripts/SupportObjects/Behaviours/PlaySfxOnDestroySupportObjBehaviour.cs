using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySfxOnDestroySupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] SFXInfo _playedSFX;
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var playSFXOriginal = original as PlaySfxOnDestroySupportObjBehaviour;
        _playedSFX = playSFXOriginal._playedSFX;
        OnDestroyed += PlaySfx;
    }

    void PlaySfx()
    {
        SoundFXManager.sm.PlaySfx(_playedSFX, ObjControl.transform.position);
    }
}
