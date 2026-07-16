using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class SpareEnemiesItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] float _startActDist;
    [SerializeField] float _actPickupMaxDist;
    [SerializeField] EnemyStatusEffectData _actingStatusEffect;
    [SerializeField] EnemyStatusEffectData _sparingStatusEffect;
    [SerializeField] CustomAnimation _actPickupAnim;
    [SerializeField] CustomAnimation _sparePickupAnim;
    [SerializeField] Vector2 _pickupColliderSize;
    [SerializeField] ParticleSystem _actParticles;
    [SerializeField] ParticleSystem _spareParticles;
    [SerializeField] SFXInfo _actSFX;
    [SerializeField] SFXInfo _spareSFX;

    EnemyControl _sparingEnemy;
    int _neededActs;
    int _currActs;
    PickupControl _currPickup;

    float _enemyHpForActCount = 100;
    float _hpNeededFor1MoreAct = 50;

    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var spareEnemiesOriginal = original as SpareEnemiesItemBehaviour;
        _startActDist = spareEnemiesOriginal._startActDist;
        _actPickupMaxDist = spareEnemiesOriginal._actPickupMaxDist;
        _actingStatusEffect = spareEnemiesOriginal._actingStatusEffect;
        _sparingStatusEffect = spareEnemiesOriginal._sparingStatusEffect;
        _actPickupAnim = spareEnemiesOriginal._actPickupAnim;
        _sparePickupAnim = spareEnemiesOriginal._sparePickupAnim;
        _pickupColliderSize = spareEnemiesOriginal._pickupColliderSize;
        _actParticles = spareEnemiesOriginal._actParticles;
        _spareParticles = spareEnemiesOriginal._spareParticles;
        _actSFX = spareEnemiesOriginal._actSFX;
        _spareSFX = spareEnemiesOriginal._spareSFX;

        behaviourManager.onUpdate += CheckEnemyDeath;
        behaviourManager.onUpdate += CheckNearbyEnemies;
        behaviourManager.onUpdate += GeneratePickup;
    }

    void CheckNearbyEnemies()
    {
        if (_sparingEnemy != null)
            return;
        var objsInRange = Physics2D.OverlapCircleAll(PlayerControl.pc.transform.position, _startActDist, Utility.GetCollidableLayers("PlayerAttack")).ToList();
        EnemyControl newSparingEnemy = objsInRange.Find(x => x.transform.root.GetComponent<EnemyControl>() != null)?.transform.root.GetComponent<EnemyControl>();
        if (newSparingEnemy == null)
            return;

        SetEnemyAsTarget(newSparingEnemy);
    }
    void CheckEnemyDeath()
    {
        if(_sparingEnemy == null && _currPickup != null)
        {
            GameObject.DestroyImmediate(_currPickup.gameObject);
            _currPickup = null;
        }
    }

    void SetEnemyAsTarget(EnemyControl targetedEnemy)
    {
        _sparingEnemy = targetedEnemy;
        _sparingEnemy.StatusEffectManager.AddEffects(_actingStatusEffect.StatusEffects, _actingStatusEffect);
        int enemyMaxHP = targetedEnemy.EnemyHP.MaxHP;
        _currActs = 0;
        _neededActs = 1;
        if (enemyMaxHP < _enemyHpForActCount)
        {
            _sparingEnemy.StatusEffectManager.AddEffects(_sparingStatusEffect.StatusEffects, _sparingStatusEffect);
            return;
        }

        _neededActs += (int)((enemyMaxHP - _enemyHpForActCount) / _hpNeededFor1MoreAct) + 1;

    }

    void GeneratePickup()
    {
        if (_sparingEnemy == null || _currPickup != null)
            return;
        var tilesInRange = MapManager.mm.LoadedTiles.Where(tile => Vector3.Distance(tile.transform.position, PlayerControl.pc.transform.position) <= _actPickupMaxDist).ToList();
        Vector2 actPosition = tilesInRange.Count == 0 ? PlayerControl.pc.transform.position : tilesInRange[Random.Range(0, tilesInRange.Count)].transform.position;
        PickupControl pickupControl = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["Pickup"], actPosition, Quaternion.identity).GetComponent<PickupControl>();
        _currPickup = pickupControl;
        CustomAnimation pickupAnim;
        if(_currActs != _neededActs -1)
        {
            pickupControl.Pickup.OnPickup += Act;
            pickupAnim = _actPickupAnim;
        }
        else
        {
            pickupControl.Pickup.OnPickup += SpareEnemy;
            pickupAnim = _sparePickupAnim;
        }
        pickupControl.Animator.AddAnimations(new() { pickupAnim });
        pickupControl.Animator.ChangeAnim(pickupAnim.AnimationName);
        pickupControl.Collider.size = _pickupColliderSize;
    }

    void Act(Pickup pickup)
    {
        _currActs++;
        if(_currActs == _neededActs -1)
            _sparingEnemy.StatusEffectManager.AddEffects(_sparingStatusEffect.StatusEffects, _sparingStatusEffect);
        //generate act particles on enemy
        ParticleConfig actParticles = new(_actParticles, Vector2.zero, Quaternion.identity, _actParticles.main.duration, _sparingEnemy.transform, true, true);
        ParticleManager.pm.SpawnParticles(actParticles);
        SoundFXManager.sm.PlaySfx(_actSFX, PlayerControl.pc.transform.position);
    }

    void SpareEnemy(Pickup pickup)
    {
        //deal damage equal to the enemy health
        EnemyHP enemyHp = _sparingEnemy.EnemyHP;
        int enemyMaxHp = enemyHp.MaxHP;
        enemyHp.TakeDamage(enemyMaxHp + 100);
        //create particles on enemy pos
        Vector2 particlesPosition = _sparingEnemy.transform.position;
        ParticleConfig spareParticles = new(_spareParticles, particlesPosition, Quaternion.identity, _spareParticles.main.duration);
        ParticleManager.pm.SpawnParticles(spareParticles);
        _sparingEnemy = null;
        if(_currPickup != null)
            GameObject.Destroy(_currPickup.gameObject);

        SoundFXManager.sm.PlaySfx(_spareSFX, PlayerControl.pc.transform.position);

    }

    public override void RemoveBehaviour()
    {
        if (_currPickup != null)
            GameObject.Destroy(_currPickup.gameObject);
    }
}
