using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ElectricHitAttackEffect : AttackEffect
{
    new public static bool isUsable => true;

    [SerializeField] int _maxElectricRays;
    [SerializeField] float _maxDistBetweenEnemies;
    [Range(0f, 100f)]
    [SerializeField] float _damagePercentage;
    [SerializeField] CustomLineInterface _zapVfxInterface;
    [SerializeField] float _zapVfxDurationMin;
    [SerializeField] float _zapVfxDurationMax;
    
    float _damageMultiplier => _damagePercentage / 100;
    public ElectricHitAttackEffect(ElectricHitAttackEffect original, Attack affectedAttack) : base(original, affectedAttack)
    {
        
    }
    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var electricHitOriginal = (ElectricHitAttackEffect)original;
        _maxElectricRays = electricHitOriginal._maxElectricRays;
        _maxDistBetweenEnemies = electricHitOriginal._maxDistBetweenEnemies;
        _damagePercentage = electricHitOriginal._damagePercentage;
        _zapVfxInterface = electricHitOriginal._zapVfxInterface;
        _zapVfxDurationMin = electricHitOriginal._zapVfxDurationMin;
        _zapVfxDurationMax = electricHitOriginal._zapVfxDurationMax;

        OnEnemyHit += ElectrifyNearbyEnemies;
    }
    void ElectrifyNearbyEnemies(EnemyControl mainEnemy)
    {
        var reachableEnemies = new List<GameObject>(WaveManager.wm.Enemies.Where(x => Vector2.Distance(x.transform.position, mainEnemy.transform.position) <= _maxDistBetweenEnemies));
        var orderedEnemies = Utility.GetClosestTo(reachableEnemies, mainEnemy.transform);
        if (reachableEnemies.Count == 0)
            return;
        for(int i = 0; i < _maxElectricRays; i++)
        {
            var reachedEnemyIndex = i >= reachableEnemies.Count ? Random.Range(0, reachableEnemies.Count) : i;
            var reachedEnemy = reachableEnemies[reachedEnemyIndex];
            ZapEnemy(mainEnemy, reachedEnemy.GetComponent<EnemyControl>());
        }

    }
    void ZapEnemy(EnemyControl mainEnemy, EnemyControl zappedEnemy)
    {
        zappedEnemy.EnemyHP.TakeDamage((int)(AffectedAttack.ParentWeapon.Damage * _damageMultiplier));
        var vfx = new GameObject("ElectricHitVFX");
        var lr = vfx.AddComponent<CustomLineRenderer>();
        //lr.SetLineData(, _zapVfxInterface.isVertical, _zapVfxInterface.tileSize);
        lr.GenerateLine(mainEnemy.transform.position, zappedEnemy.transform.position, _zapVfxInterface);
        GameObject.Destroy(vfx, Random.Range(_zapVfxDurationMin, _zapVfxDurationMax));
    }
}
