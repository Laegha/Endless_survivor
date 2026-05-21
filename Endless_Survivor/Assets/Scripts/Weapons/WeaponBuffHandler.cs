using System;
using System.Collections.Generic;

class WeaponBuffHandler
{
    public List<WeaponAttackManager> buffedWeapons;
    public WeaponStats statsBuff;
    int _enemyKillsNeeded;
    float _timeDuration;
    public Action callbackOnEnd;

    float _enemyKillCounter = 0;
    public enum BuffDurationType
    {
        ByEnemyKills,
        ByTime,
        WaitForExternal
    }

    public WeaponBuffHandler(List<WeaponAttackManager> buffedWeapons, WeaponStats statsBuff, BuffDurationType durationType, int enemyKillsNeeded, float timeDuration, Action callbackOnEnd = null)
    {
        this.buffedWeapons = buffedWeapons;
        this.statsBuff = statsBuff;
        _enemyKillsNeeded = enemyKillsNeeded;
        _timeDuration = timeDuration;
        this.callbackOnEnd = callbackOnEnd;
        BuffWeapons();
        if (durationType == BuffDurationType.WaitForExternal)
            return;
        if(durationType == BuffDurationType.ByEnemyKills)
        {
            EnemySpawnManager.esm.OnEnemySpawned += AddDeathCallbackToEnemy;
            return;
        }
        if(durationType == BuffDurationType.ByTime)
        {
            GameManager.gm.DelayAction(_timeDuration, DebuffWeapons, null);
            return;
        }

    }
    public void BuffWeapons()
    {
        foreach (var weapon in buffedWeapons)
        {
            if (weapon == null) continue;
            weapon.WeaponStats.TemporalStatIncrease(statsBuff, false);
        }
    }

    public void DebuffWeapons()
    {
        foreach (var weapon in buffedWeapons)
        {
            if (weapon == null) continue;
            weapon.WeaponStats.TemporalStatIncrease(statsBuff, true);
        }
        callbackOnEnd?.Invoke();
    }

    void AddDeathCallbackToEnemy(EnemyControl enemy)
    {
        enemy.EnemyHP.OnDeath += IncreaseEnemyKillCounter;
    }

    void IncreaseEnemyKillCounter(EnemyControl placeholder)
    {
        _enemyKillCounter++;
        if(_enemyKillCounter >= _enemyKillsNeeded)
        {
            _enemyKillCounter = 0;
            DebuffWeapons();
        }
    }
}