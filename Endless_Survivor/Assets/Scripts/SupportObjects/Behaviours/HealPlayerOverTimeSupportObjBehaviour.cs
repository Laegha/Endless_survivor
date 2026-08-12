using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPlayerOverTimeSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] RandomBetweenTwoConstants _healAmmount;
    [SerializeField] RandomBetweenTwoConstants _timeBetweenHeals;
    [SerializeField] DirectionalCustomAnimation _animationsFacingPlayer;
    [SerializeField] int _healFrame;
    float _healTimer;
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var healPlayerOriginal = original as HealPlayerOverTimeSupportObjBehaviour;
        _healAmmount = healPlayerOriginal._healAmmount;
        _timeBetweenHeals = healPlayerOriginal._timeBetweenHeals;
        _animationsFacingPlayer = new(ObjControl.Animator, healPlayerOriginal._animationsFacingPlayer);
        _healFrame = healPlayerOriginal._healFrame;
        foreach (var animation in _animationsFacingPlayer.NonNullAnimations)
        {
            animation.Events.Add(new(null, _healFrame, ApplyHeal));
        }
        ObjControl.Animator.AddAnimations(new(_animationsFacingPlayer.NonNullAnimations));
        _healTimer = _timeBetweenHeals.rand;

        OnUpdate += ReduceTimer;
    }

    void ReduceTimer()
    {
        _healTimer -= Time.deltaTime;
        if (_healTimer > 0)
            return;
        _healTimer = _timeBetweenHeals.rand;
        if (PlayerControl.pc.PlayerHPManager.RemainingHP == PlayerControl.pc.PlayerHPManager.MaxHP)
            return;
        Vector2 playerDir = PlayerControl.pc.transform.position - ObjControl.transform.position;
        ObjControl.Animator.ChangeAnim(_animationsFacingPlayer.GetAnim(playerDir));
    }
    void ApplyHeal()
    {

        PlayerControl.pc.PlayerHPManager.Heal((int)_healAmmount.rand);
    }
}
