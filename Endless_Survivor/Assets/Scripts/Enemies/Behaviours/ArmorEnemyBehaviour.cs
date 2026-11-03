using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => 1;
    [Range(0,100)][SerializeField] float _maxRecievedDamagePercent;
    float _originalDmgMultiplier;
    float _maxDamage => _maxHp * _maxRecievedDamagePercent / 100; 
    int _maxHp;
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var armorOriginal = original as ArmorEnemyBehaviour;
        _maxRecievedDamagePercent = armorOriginal._maxRecievedDamagePercent;
        EnemyControl.EnemyHP.OnDamageMultiplierChanged += UpdateOriginalMultiplier;
        EnemyControl.EnemyHP.OnDamageTaken += ScaleArmor;
    }
    public override void Start()
    {
        base.Start();
        _maxHp = EnemyControl.EnemyHP.MaxHP;
        UpdateOriginalMultiplier();
    }
    void ScaleArmor(int incomingDamage)
    {
        float maxMultiplier = _maxDamage / incomingDamage;
        float armorMultiplier = Mathf.Clamp(maxMultiplier, EnemyHP.minDmgMultiplier, _originalDmgMultiplier);//max damage can be overriden if incomingDamage * EnemyHP.minDmgMultiplier is greater than maxDamage

        EnemyControl.EnemyHP.SoftChangeDamageMultiplier(armorMultiplier);
    }
    void UpdateOriginalMultiplier() => _originalDmgMultiplier = EnemyControl.EnemyHP.IncomingDamageMultiplier;
}
