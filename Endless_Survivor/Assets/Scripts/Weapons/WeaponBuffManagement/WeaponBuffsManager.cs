using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponBuffsManager : MonoBehaviour
{
    static WeaponBuffsManager instance;

    public static WeaponBuffsManager wbm {  get { return instance; } }
    List<WeaponBuffHandler> _activeBuffs;
    Dictionary<WeaponBuffHandler, PlayerGFXChanger> _gfxChangers = new();
    
    List<WeaponBuffHandler> _timeBasedHandlers => new(_activeBuffs.Where(x => x.BuffData.DurationType == WeaponBuffHandler.BuffDurationType.ByTime).ToList());
    List<WeaponBuffHandler> _killBasedHandlers => new(_activeBuffs.Where(x => x.BuffData.DurationType == WeaponBuffHandler.BuffDurationType.ByEnemyKills).ToList());

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        EnemySpawnManager.esm.OnEnemySpawned += AddDeathCallbackToEnemy;
        PlayerControl.pc.WeaponManager.OnWeaponPickup += UpdateWeaponsOnHandlers;
    }

    void UpdateWeaponsOnHandlers()
    {
        foreach (var buffHandler in _activeBuffs)
        {
            buffHandler.UpdateWeaponsList();
            buffHandler.UpdateAllWeaponsBuffs();
        }
    }

    public bool AddBuffStack(WeaponBuffData buffData)
    {
        var activeBuff = _activeBuffs.Find(x => x.BuffData == buffData);
        if (activeBuff == null)
        {
            activeBuff = new(buffData);
            _activeBuffs.Add(activeBuff);
            PlayerGFXChanger buffGfxChangerInstance = new(buffData.OnBuffPlayerGfxChanger);
            buffGfxChangerInstance.ApplyGFX();
            _gfxChangers.Add(activeBuff, buffGfxChangerInstance);
            SoundFXManager.sm.PlaySfx(buffData.OnBuffSFX, PlayerControl.pc.transform.position);
        }
        if(activeBuff.MyBuffStacks >= buffData.BuffMaxStacks)
            return false;   
        activeBuff.AddStack();
        return true;
    }
    public void RemoveBuffStack(WeaponBuffData buffData) => RemoveBuffStack(_activeBuffs.Find(x => x.BuffData == buffData));
    void RemoveBuffStack(WeaponBuffHandler buffHandler)
    {
        buffHandler.RemoveStack();
        if (buffHandler.MyBuffStacks > 0)
            return;
        //Remove buff entirely
        buffHandler.RemoveBuffCompletely();
        _activeBuffs.Remove(buffHandler);
        _gfxChangers[buffHandler].UnApplyGFX();
        _gfxChangers.Remove(buffHandler);

    }
    private void Update()
    {
        foreach (var buffHandler in _timeBasedHandlers)
        {
            if (!buffHandler.DecreaseTimer())
                continue;
            RemoveBuffStack(buffHandler);
        }
    }

    void AddDeathCallbackToEnemy(EnemyControl enemy)
    {
        enemy.EnemyHP.OnDeath += IncreaseEnemyKillCounter;
    }

    void IncreaseEnemyKillCounter(EnemyControl placeholder)
    {
        foreach (var buffHandler in _killBasedHandlers)
        {
            if (!buffHandler.KilledEnemy())
                continue;
            RemoveBuffStack(buffHandler);
        }
    }
}
