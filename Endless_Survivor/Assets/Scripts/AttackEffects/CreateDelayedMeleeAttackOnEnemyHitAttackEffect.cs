using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDelayedMeleeAttackOnEnemyHitAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    [SerializeField] float _creationDelay;
    [SerializeField] MeleeData _meleeData;
    [SerializeField] int _applyDamageFrame;
    [Tooltip("The parecentaje of the original attack's damage")]
    [Range(0,100)][SerializeField] float _damagePercent;
    [SerializeField] DamageInfo.DamageType _damageType;
    [SerializeField] float _knockBack;
    [SerializeField] SFXInfo _onAttackSFX;
    public CreateDelayedMeleeAttackOnEnemyHitAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }

    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var createMeleeOriginal = original as CreateDelayedMeleeAttackOnEnemyHitAttackEffect;
        _creationDelay = createMeleeOriginal._creationDelay;
        _meleeData = createMeleeOriginal._meleeData;
        _applyDamageFrame = createMeleeOriginal._applyDamageFrame;
        _damagePercent = createMeleeOriginal._damagePercent;
        _damageType = createMeleeOriginal._damageType;
        _knockBack = createMeleeOriginal._knockBack;
        _onAttackSFX = createMeleeOriginal._onAttackSFX;

        OnEnemyHit += StartDelay;

    }

    void StartDelay(EnemyControl hitEnemy)
    {
        GameManager.gm.DelayAction(_creationDelay, () => CreateAttack(hitEnemy), () => PlayerControl.pc == null);
    }
    void CreateAttack(EnemyControl hitEnemy)
    {
        MeleeAttack meleeAttack = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["Melee"], AffectedAttack.transform.position, Quaternion.identity).GetComponent<MeleeAttack>();
        meleeAttack.IsSecondaryAttack = true;
        meleeAttack.TriggersPassiveItemHit = false;
        
        if(hitEnemy != null)
        {
            meleeAttack.transform.SetParent(hitEnemy.transform, true);
            Vector2 enemyDirection = (hitEnemy.transform.position - AffectedAttack.transform.position).normalized;
            float attackRotation = Mathf.Atan2(enemyDirection.y, enemyDirection.x) * Mathf.Rad2Deg;
            meleeAttack.transform.rotation = Quaternion.Euler(0,0, attackRotation);
            meleeAttack.VfxRenderer.flipY = enemyDirection.x < 0;

        }
        DamageInfo attackDamageInfo = new(AffectedAttack.AttackDamage * _damagePercent / 100, _damageType);
        meleeAttack.StartAttack((int)attackDamageInfo.CalculatedDamage, _knockBack, _meleeData);
        float damageDelay = _applyDamageFrame / _meleeData.AttackVfxAnimation.FramesPerSecond;
        GameManager.gm.DelayAction(damageDelay, meleeAttack.ApplyDamage, () => PlayerControl.pc == null);
        GameObject.Destroy(meleeAttack.gameObject, _meleeData.AttackVfxAnimation.AnimDuration);
        SoundFXManager.sm.PlaySfx(_onAttackSFX, PlayerControl.pc.transform.position);
    }
}
