using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerAnimsOnActivateItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [Tooltip("WARNING: The names must be the same as the animations the they override, or else they won't change")]
    [SerializeField] List<CustomAnimation> _overridingAnimations;

    public override void Activate()
    {
        base.Activate();
        ChangeAnimations(_overridingAnimations);

    }
    void ChangeAnimations(List<CustomAnimation> newAnimations)
    {
        var playerAnims = PlayerControl.pc.PlayerAnimator.Animations;
        for (int i = 0; i < playerAnims.Count; i++)
        {
            CustomAnimation correspondingAnim = newAnimations.Find(x => x.AnimationName == playerAnims[i].AnimationName);
            if(correspondingAnim == null) 
                continue;
            playerAnims[i] = correspondingAnim;

        }
    }
    public override void RemoveBehaviour()
    {

    }
}
