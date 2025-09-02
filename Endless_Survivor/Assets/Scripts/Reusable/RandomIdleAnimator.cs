using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomIdleAnimator : MonoBehaviour
{
    CustomAnimator _animator;
    Roulette<CustomAnimation> _animationRoulette;
    float _randomAnimChance;
    float _randomAnimTime;

    float _animTimer;
    bool _isPlayingAnim = false;
    
    public void SetData(CustomAnimator animator, float animChance, float animTime, List<RandomIdleAnimation> animations)
    {
        _animator = animator;
        _randomAnimChance = animChance;
        _randomAnimTime = animTime;
        Dictionary<CustomAnimation, int> rouletteELements = new Dictionary<CustomAnimation, int>();
        foreach (var anim in animations)
        {
            CustomAnimation newAnim = new(_animator, anim);
            newAnim.Events.Add(new(null, newAnim.Frames.Length-1, ReturnToDefaultIdle));
            rouletteELements.Add(newAnim, anim.AnimationWeight);

        }
        _animator.AddAnimations(rouletteELements.Keys.ToList());
        _animationRoulette = new Roulette<CustomAnimation>(rouletteELements);
    }
    void ReturnToDefaultIdle()
    {
        print("Random animation ended");
        _animator.ChangeAnim("Idle");
    }
    private void Update()
    {
        if (_isPlayingAnim)
            return;

        _animTimer -= Time.deltaTime;
        if (_animTimer > 0)
            return;

        _animTimer = _randomAnimTime;
        int rand = Random.Range(0, 101);
        if (rand > _randomAnimChance)
            return;

        PlayRandomAnim();

    }
    void PlayRandomAnim()
    {
        var anim = _animationRoulette.Spin();
        _animator.ChangeAnim(anim.AnimationName);
        _isPlayingAnim = true;
        StartCoroutine(StopPlayingAnim(anim.AnimDuration));
    }
    IEnumerator StopPlayingAnim(float animDuration)
    {
        yield return new WaitForSeconds(animDuration);
        _isPlayingAnim = false;
    }
        
}
