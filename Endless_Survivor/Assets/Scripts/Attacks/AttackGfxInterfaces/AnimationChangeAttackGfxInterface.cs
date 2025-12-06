using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimationChangeAttackGfxInterface : AttackGfxInterface
{
    public CustomAnimation newAnimation;
    public Material newMaterial;
    public ParticleSystem particles;

    public void ChangeAttackGfx(CustomAnimator attackAnimator, SpriteRenderer[] renderers)
    {
        if(newAnimation?.Frames.Length > 0)
        {
            attackAnimator.AddAnimations(new List<CustomAnimation> { newAnimation });
            attackAnimator.ChangeAnim(newAnimation.AnimationName, true, false);
        }
        if(newMaterial != null)
        {
            foreach (var renderer in renderers)
            {
                renderer.material = newMaterial;
            }
        }
        if(particles != null)
        {
            ParticleManager.pm.SpawnParticles(new(particles, attackAnimator.transform.position, attackAnimator.transform.rotation, -1, attackAnimator.transform, true, true));
        }
    }
}
