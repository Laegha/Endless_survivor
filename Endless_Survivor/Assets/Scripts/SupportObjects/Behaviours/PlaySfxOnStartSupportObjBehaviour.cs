using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlaySfxOnStartSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] SFXInfo _playedSFX;
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var playSFXOriginal = original as PlaySfxOnStartSupportObjBehaviour;
        _playedSFX = playSFXOriginal._playedSFX;
        OnStart += PlaySfx;
    }

    void PlaySfx()
    {
        SoundFXManager.sm.PlaySfx(_playedSFX, ObjControl.transform.position);
    }
}
