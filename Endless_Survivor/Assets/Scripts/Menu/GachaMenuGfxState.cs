using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class GachaMenuGfxState
{
    public string stateName;
    public bool isSkipable = true;
    public bool endsByTime = true;
    public float delayAfterEnding = 0;
    public SerializedDictionary<GameObject, bool> startObjectActivations;
    public SerializedDictionary<Animator, string> startAnimations;
    public SerializedDictionary<GameObject, bool> onSkipObjectActivations;
    public SerializedDictionary<Animator, string> onSkipAnimations;
    public UnityEvent startEvents;
    public List<ChainAnimation> chainAnimations;

    bool _wasSkiped;
    public float stateDuration
    {
        get
        {
            List<float> animationDurations = new List<float>();
            foreach (var chainAnim in chainAnimations)
            {
                var headAnimDuration = Utility.GetClipFromAnimator(chainAnim.headAnim.animator, chainAnim.headAnim.animName).length;
                var tailAnimDuration = Utility.GetClipFromAnimator(chainAnim.tailAnim.animator, chainAnim.tailAnim.animName).length;
                var chainDuration = headAnimDuration + tailAnimDuration + chainAnim.delay;
                animationDurations.Add(chainDuration);
            }
            foreach(var startAnim in startAnimations)
            {
                var clip = Utility.GetClipFromAnimator(startAnim.Key, startAnim.Value);
                var animDuration = clip != null ? clip.length : 0;
                animationDurations.Add(animDuration);
            }
            if (stateName == "Gaching")
                Debug.Log("THE DURATION OF THE STATE IS " + animationDurations.Max());
            return animationDurations.Count > 0 ? animationDurations.Max() : 0;
        }
    }

    public void StartState()
    {
        _wasSkiped = false;
        foreach (var obj in startObjectActivations)
        {
            obj.Key.SetActive(obj.Value);
        }
        foreach(var anim in startAnimations)
        {
            anim.Key.Play(anim.Value);
        }
        foreach(var chainAnim in chainAnimations)
        {
            var duration = Utility.GetClipFromAnimator(chainAnim.headAnim.animator, chainAnim.headAnim.animName).length + chainAnim.delay;
            GameManager.gm.RoutineRunner(PlayChainAnimation(duration, chainAnim.tailAnim));
        }
        startEvents?.Invoke();
    }

    public void SkipState()
    {
        _wasSkiped = true;
        foreach(var anim in onSkipAnimations)
        {
            anim.Key.Play(anim.Value, 0, 1);
        }
        foreach (var obj in onSkipObjectActivations)
        {
            obj.Key.SetActive(obj.Value);
        }
    }
    IEnumerator PlayChainAnimation(float delay, ChainAnimationPart tailAnimation)
    {
        for(float i = 0; i <= delay; i+= Time.deltaTime)
        {
            if(_wasSkiped)
                yield break;
            yield return null;
        }
        if(!_wasSkiped)
            tailAnimation.animator.Play(tailAnimation.animName);
    }
}

[Serializable]
public class ChainAnimation
{
    public ChainAnimationPart headAnim;
    public ChainAnimationPart tailAnim;
    public float delay;
}
[Serializable]
public class ChainAnimationPart
{
    public Animator animator;
    public string animName;
}