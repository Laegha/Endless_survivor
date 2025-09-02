using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class AnimationEvent
{
    public int frameIndex;
    public Action frameAction;

    public AnimationEvent(AnimationEvent original = null, int frameIndex = 0, Action frameAction = null)
    {
        if (original == null)
        {
            this.frameIndex = frameIndex;
            this.frameAction = frameAction;
            return;
        }
        this.frameIndex = original.frameIndex;
        this.frameAction = original.frameAction;
        //if (original.frameAction != null)
            //foreach (var action in original.frameAction.GetInvocationList())
                //frameAction += (Action)action;
    }
}
