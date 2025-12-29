using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerStatusEffect
{
    public static int maxStacks => 0;
    PlayerStatusEffectGroup _thisGroup;
    Func<bool> _endCondition;
    bool _ended = false;
    public PlayerStatusEffectGroup ThisGroup { get { return _thisGroup; } set { _thisGroup = value; } }
    public Func<bool> EndCondition { get { return _endCondition; } set { _endCondition = value; } }

    public virtual void Initialize(PlayerStatusEffect original)
    {

    }

    public virtual void Start() { }
    public virtual void Update()
    {
        if (_endCondition != null && _endCondition() && !_ended)
        {
            _ended = true;
            PlayerControl.pc.StatusEffectManager.RemoveEffect(this);
            return;
        }
    }
    public virtual void End() { }
    public virtual void PlayerHit() { }
}
