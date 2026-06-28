using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParryDamageEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => 1;
    [Range(0, 100)][SerializeField] float _maxParryChance;
    [SerializeField] float _parryChanceIncreasePerFailedParry;
    [SerializeField] float _parryChanceDecreasePerSuccesfullParry;
    [SerializeField] Vector2 _parryAreaSize;
    [SerializeField] Vector2 _parryAreaOffset;
    [SerializeField] CapsuleDirection2D _areaDirection;
    [SerializeField] LayerMask _parryLayer;
    [SerializeField] DirectionalCustomAnimation _parryAnimations;
    [SerializeField] float _parryDuration;
    float _currParryChance;

    List<Attack> _failedParryAttacks = new List<Attack>();
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var parryOriginal = original as ParryDamageEnemyBehaviour;
        _maxParryChance = parryOriginal._maxParryChance;
        _parryChanceIncreasePerFailedParry = parryOriginal._parryChanceIncreasePerFailedParry;
        _parryChanceDecreasePerSuccesfullParry = parryOriginal._parryChanceDecreasePerSuccesfullParry;
        _parryAreaSize = parryOriginal._parryAreaSize;
        _parryAreaOffset = parryOriginal._parryAreaOffset;
        _areaDirection = parryOriginal._areaDirection;
        _parryLayer = parryOriginal._parryLayer;
        _parryAnimations = new(EnemyControl.Animator, parryOriginal._parryAnimations);
        EnemyControl.Animator.AddAnimations(_parryAnimations.NonNullAnimations);
        _parryDuration = parryOriginal._parryDuration;
    }

    public override void PassiveUpdate()
    {
        base.PassiveUpdate();
        if (IsActive)
            return;
        var objsInArea = Physics2D.OverlapCapsuleAll(EnemyControl.transform.position, _parryAreaSize, _areaDirection, 0, _parryLayer).ToList();
        List<Attack> attacksInArea = objsInArea.Where(obj => obj.GetComponent<Attack>() != null).Select(obj => obj.GetComponent<Attack>()).ToList();
        foreach(var attack in attacksInArea)
        {
            CheckParry(attack);
        }
    }

    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        //add a "parrying" state, where every attack that enters the area gets parried
        var objsInArea = Physics2D.OverlapCapsuleAll(EnemyControl.transform.position, _parryAreaSize, _areaDirection, 0).ToList();
        List<Attack> attacksInArea = objsInArea.Where(obj => obj.GetComponent<Attack>() != null).Select(obj => obj.GetComponent<Attack>()).ToList();
        foreach(var attack in attacksInArea)
        {
            GameObject.Destroy(attack.gameObject);
        }
    }


    void CheckParry(Attack parryingAttack)
    {
        if (_failedParryAttacks.Contains(parryingAttack))//the attack was already not parried, checking every frame would make no sense
            return;
        float rand = Random.Range(0, 100);
        if(rand > _currParryChance)
        {
            if (_currParryChance < _maxParryChance)
                _currParryChance = Mathf.Clamp(_currParryChance + _parryChanceIncreasePerFailedParry, 0, _maxParryChance);
            return;
        }
        _currParryChance = Mathf.Clamp(_currParryChance - _parryChanceDecreasePerSuccesfullParry, 0, _maxParryChance);
        //activate parry state
        EnemyControl.BehaviourManager.ActivateBehaviour(this);
        Vector2 parryDirection = (parryingAttack.transform.position - EnemyControl.transform.position).normalized;
        CustomAnimation parryingAnim = _parryAnimations.GetAnim(parryDirection);
        EnemyControl.Animator.ChangeAnim(parryingAnim);
        GameManager.gm.DelayAction(_parryDuration, EndParry, () => !IsActive);
    }

    void EndParry()
    {
        if(_parryAnimations.NonNullAnimations.Any(x => x.AnimationName == EnemyControl.Animator.CurrAnim.AnimationName))
        {
            EnemyControl.Animator.EndAnimation(EnemyControl.Animator.CurrAnim);
        }
        KillBehaviour();
    }
}
