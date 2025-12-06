using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class MeleeWeaponAttackController : WeaponAttackController
{
    new public static bool isUsable => true;
    [SerializeField]MeleeData _meleeData;
    TransformMover _currHandMover = null;
    Transform _hand;
    Vector2 _originalHandLocalPos;
    Action _onAttackApply;

    float _weaponStopDist
    {
        get
        {
            if (_meleeData.IsCircle)
                return _meleeData.CircleRadius * _handStopDistFactor;
            else
                return Mathf.Min(_meleeData.BoxSize.x, _meleeData.BoxSize.y) * _handStopDistFactor;
        }
    }

    readonly static float _handSpeed = 15;
    readonly static float _handStopDistFactor = .7f;

    public Action OnAttackApply {get { return _onAttackApply; } set { _onAttackApply = value; } }

    public override void Initialize(WeaponControl weaponControl, WeaponAttackController original)
    {
        base.Initialize(weaponControl, original);
        var meleeWeaponOriginal = original as MeleeWeaponAttackController;
        _meleeData = meleeWeaponOriginal._meleeData;

        _hand = WeaponControl.transform.parent;
        _originalHandLocalPos = _hand.localPosition;
        WeaponControl.WeaponAim.ChangeDistCheckPos(() => { return (Vector2)PlayerControl.pc.transform.position;/* + _originalHandLocalPos;*/ });
        InitializeAttack += InitiateMelee;
    }
    public override void Update()
    {
        base.Update();
        if (_currHandMover == null)
            return;
        if (_currHandMover.id != "Return")
        {
            if(!WeaponControl.WeaponAttackManager.InRange)
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
    public override void StartAttack()
    {
        base.StartAttack();
        if (WeaponControl.WeaponAnimator.CurrAnim.AnimationName == AnimationName)
        {
            Attack();
            return;
        }

        Vector2 enemyPos = WeaponControl.WeaponAim.CurrTrackingEnemyHit.point;
        Vector2 handMovement = enemyPos - (Vector2)_hand.position;
        WeaponControl.WeaponAttackManager.PauseAttackCooldown();
        float dist = handMovement.magnitude - _weaponStopDist;
        TransformMover attackTrMover = new("Attack", handMovement.normalized, dist, _handSpeed, _hand, WeaponControl.WeaponAim.CurrTrackingEnemyHit.collider.transform, PlayAttackAnimation);
        _currHandMover = attackTrMover;
    }
    public override void End()
    {
        base.End();
        EndReturnMovement();
    }

    void PlayAttackAnimation()
    {
        var attackAnimDuration = WeaponControl.WeaponAnimator.Animations.Find(x => x.AnimationName == AnimationName).AnimDuration;
        WeaponControl.WeaponAnimator.ChangeAnim(AnimationName);
        WeaponControl.WeaponAttackManager.UnPauseAttackCooldown();

        MeleeAttack meleeAttack = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["Melee"], WeaponControl.transform.position, WeaponControl.transform.rotation).GetComponent<MeleeAttack>();
        meleeAttack.transform.SetParent(WeaponControl.transform, true);

        InitializeAttack(meleeAttack);

        //OverrideAttackCooldown(Mathf.Clamp(AttackCooldown, attackAnimDuration, AttackCooldown));//ensuring that the attack can't  be faster than the animation to avoid visual glitches
        _currHandMover = null;
        GameManager.gm.DelayAction(attackAnimDuration, () => {ReturnToOriginalPos();/* UnPauseAttackCooldown();*/ }, () => this == null);
        GameManager.gm.RoutineRunner(StuckHandInAttackPos(_hand.position, attackAnimDuration));
    }
    IEnumerator StuckHandInAttackPos(Vector2 attackPos, float attackDuration)
    {
        float lapsedTime = 0;
        while(lapsedTime < attackDuration)
        {
            yield return null;
            lapsedTime += Time.deltaTime;
            _hand.position = attackPos;
        }
    }
    public override void Attack()
    {
        base.Attack();
        _onAttackApply?.Invoke();
    }
    void InitiateMelee(Attack attack)
    {
        var meleeAttack = attack as MeleeAttack;
        meleeAttack.StartAttack((int)Damage, WeaponStats.Knockback, _meleeData);
    }

    void ReturnToOriginalPos()
    {
        WeaponControl.WeaponAttackManager.UnPauseAttackCooldown();
        Vector2 returnVector = _originalHandLocalPos - (Vector2)_hand.localPosition;
        TransformMover returnMover = new("Return", returnVector.normalized, returnVector.magnitude, _handSpeed, _hand, null, EndReturnMovement);
        _currHandMover = returnMover;
    }
    void UpdateMoverDist()
    {
        var newDir = (_currHandMover.destinationTarget.position - _hand.position).normalized;
        var objsInDir = Physics2D.RaycastAll(WeaponControl.transform.position, newDir, Mathf.Infinity, Utility.GetCollidableLayers("PlayerAttack")).ToList();
        var newPoint = objsInDir.Find(x => x.collider.transform == _currHandMover.destinationTarget).point;
        var newDist = (newPoint - (Vector2)_hand.position).magnitude + _currHandMover.lapsedDistance;
        _currHandMover.distance = newDist - _weaponStopDist;
        _currHandMover.direction = newDir;
    }
    void EndReturnMovement()
    {
        _hand.localPosition = _originalHandLocalPos; 
        _currHandMover = null;

        WeaponControl.WeaponAttackManager.FinishAttack();
    }
}

