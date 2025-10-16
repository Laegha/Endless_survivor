using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    TransformMover _currHandMover = null;
    Transform _hand;
    Vector2 _originalHandLocalPos;
    MeleeData _meleeData;

    readonly static float _handSpeed = 15;
    readonly static float _handStopDistFactor = .5f;

    public MeleeData MeleeData { set { _meleeData = value; } }
    public override void Start()
    {
        base.Start();
        _hand = transform.parent;
        _originalHandLocalPos = _hand.localPosition;
        WeaponControl.WeaponAim.ChangeDistCheckPos(() => {return (Vector2)PlayerControl.pc.transform.position + _originalHandLocalPos; });
        InitializeAttack += InitiateMelee;
    }
    public override void Update()
    {
        base.Update();
        if (_currHandMover == null)
            return;
        if (_currHandMover.id != "Return")
        {
            if(!InRange)
            {
                ReturnToOriginalPos();

            }
            else
            {
                UpdateMoverDist();
            }
        }
        _currHandMover.Update();
    }
    public override void Attack()
    {
        base.Attack();
        if (WeaponControl.WeaponAnimator.CurrAnim.AnimationName == "Attack")
        {
            CreateMeleeAttack();
            return;
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, Mathf.Infinity, Utility.GetCollidableLayers("PlayerAttack"));
        Vector2 enemyPos = hit.point;
        Vector2 handMovement = enemyPos - (Vector2)_hand.position;
        PauseAttackCooldown();
        float dist = handMovement.magnitude /*- (_meleeData.IsCircle ? _meleeData.CircleRadius : _meleeData.BoxSize.y) * _handStopDistFactor*/;
        TransformMover attackTrMover = new("Attack", handMovement.normalized, dist, _handSpeed, _hand, PlayAttackAnimation);
        _currHandMover = attackTrMover;
    }

    void PlayAttackAnimation()
    {
        var attackAnimDuration = WeaponControl.WeaponAnimator.Animations.Find(x => x.AnimationName == "Attack").AnimDuration;
        WeaponControl.WeaponAnimator.ChangeAnim("Attack");
        UnPauseAttackCooldown();
        //OverrideAttackCooldown(Mathf.Clamp(AttackCooldown, attackAnimDuration, AttackCooldown));//ensuring that the attack can't  be faster than the animation to avoid visual glitches

        CreateMeleeAttack();

        _currHandMover = null;
        GameManager.gm.DelayAction(attackAnimDuration, () => {ReturnToOriginalPos();/* UnPauseAttackCooldown();*/ }, null); 
    }
    void CreateMeleeAttack()
    {
        MeleeAttack meleeAttack = Instantiate(GameManager.gm.prefabHolder.Prefabs["Melee"], transform.position, transform.rotation).GetComponent<MeleeAttack>();
        meleeAttack.transform.SetParent(transform, true);

        InitializeAttack(meleeAttack);
    }
    void InitiateMelee(Attack attack)
    {
        var meleeAttack = attack as MeleeAttack;
        meleeAttack.Attack(WeaponStats.Damage, WeaponStats.Knockback, _meleeData);
    }

    void ReturnToOriginalPos()
    {
        UnPauseAttackCooldown();
        Vector2 returnVector = _originalHandLocalPos - (Vector2)_hand.localPosition;
        TransformMover returnMover = new("Return", returnVector.normalized, returnVector.magnitude, _handSpeed, _hand, () => { _hand.localPosition = _originalHandLocalPos; _currHandMover = null; });
        _currHandMover = returnMover;
    }
    void UpdateMoverDist()
    {
        var newPoint = Physics2D.Raycast(transform.position, _currHandMover.direction, Mathf.Infinity, Utility.GetCollidableLayers("PlayerAttack")).point;
        var newDist = newPoint - (Vector2)_hand.position;
        _currHandMover.distance = newDist.magnitude;
    }
}

