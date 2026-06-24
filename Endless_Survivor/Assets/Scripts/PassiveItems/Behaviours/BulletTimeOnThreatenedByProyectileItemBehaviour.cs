
using System.Collections.Generic;
using UnityEngine;

public class BulletTimeOnThreatenedByProyectileItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] float _timeScaleDecreaseOnBulletTime;
    [SerializeField] float _bulletTimeDuration;
    [SerializeField] int _parryingProyectileTargetPriority;
    [SerializeField] float _bulletDetectionDist;
    [Range(0,100)][SerializeField] float _activationChance;
    [Tooltip("A bullet will be activate if its direction is facing the player within this margin")][SerializeField] float _threatAngleRange;
    [SerializeField] GameObject _uiBulletTimeIndicator;//maybe change this for a post processing profile
    [Tooltip("Goes on top of the proyectile")][SerializeField] CustomAnimation _threatProyectileIndicator;
    [SerializeField] MaterialOverride _bulletTimePlayerMaterial;

    List<EnemyProyectile> _nonThreatProyectiles = new();
    bool _inBulletTime;
    GameObject _activeUiBulletTimeIndicator;
    GameObject _activeThreatProyectileIndicator;
    TimescaleChangeInfo _activeTimescaleChange;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var bulletTimeOnThreatenOriginal = original as BulletTimeOnThreatenedByProyectileItemBehaviour;
        _timeScaleDecreaseOnBulletTime = bulletTimeOnThreatenOriginal._timeScaleDecreaseOnBulletTime;
        _bulletTimeDuration = bulletTimeOnThreatenOriginal._bulletTimeDuration;
        _parryingProyectileTargetPriority = bulletTimeOnThreatenOriginal._parryingProyectileTargetPriority;
        _bulletDetectionDist = bulletTimeOnThreatenOriginal._bulletDetectionDist;
        _activationChance = bulletTimeOnThreatenOriginal._activationChance;
        _threatAngleRange = bulletTimeOnThreatenOriginal._threatAngleRange;
        _uiBulletTimeIndicator = bulletTimeOnThreatenOriginal._uiBulletTimeIndicator;
        _threatProyectileIndicator = new(null, bulletTimeOnThreatenOriginal._threatProyectileIndicator);
        _bulletTimePlayerMaterial = new(bulletTimeOnThreatenOriginal._bulletTimePlayerMaterial.authority, bulletTimeOnThreatenOriginal._bulletTimePlayerMaterial.material);


        GameObject uiIndicator = GameObject.Instantiate(_uiBulletTimeIndicator, GameUIManager.uiManager.transform.root);
        _activeUiBulletTimeIndicator = uiIndicator;
        _activeUiBulletTimeIndicator.transform.SetAsFirstSibling();
        _activeUiBulletTimeIndicator.SetActive(false);

        behaviourManager.onUpdate += CheckProyectiles;
    }
    void CheckProyectiles()
    {
        if (_inBulletTime)
            return;
        EnemyProyectile[] allProyectiles = GameObject.FindObjectsOfType<EnemyProyectile>();
        foreach(var proyectile in allProyectiles)
        {
            if (_nonThreatProyectiles.Contains(proyectile))
                continue;
            if (Vector2.Distance(PlayerControl.pc.transform.position, proyectile.transform.position) > _bulletDetectionDist)
                continue;
            if(!IsProyectileThreat(proyectile))
                continue;
            float rand = Random.Range(0, 100);
            if (rand > _activationChance)
                continue;
            StartBulletTime(proyectile);
            return;
        }
    }
    
    bool IsProyectileThreat(EnemyProyectile proyectile)
    {
        Rigidbody2D proyectileRb = proyectile.GetComponent<Rigidbody2D>();
        Vector2 proyectileDir = proyectileRb.velocity.normalized;
        if(proyectileDir == Vector2.zero)
            return false;
        Vector2 threatDir = PlayerControl.pc.transform.position - proyectile.transform.position;
        threatDir = threatDir.normalized;

        _nonThreatProyectiles.Add(proyectile);
        float threatAngle = Utility.GetAngleBetweenTwoVectors(proyectileDir, threatDir);
        if(Mathf.Abs(threatAngle) > _threatAngleRange)
            return false;
        return true;
    }

    void StartBulletTime(EnemyProyectile parryingProyectile)
    {
        _inBulletTime = true;
        WeaponAim.SharedAttackTargets.Add(new(parryingProyectile.gameObject, _parryingProyectileTargetPriority));
        
        if(_threatProyectileIndicator.Frames.Length > 0)
        {
            AnimatedObjConfig indicatorConfig = new(_threatProyectileIndicator, Vector2.zero, Quaternion.identity, -1, parryingProyectile.transform, true, true);
            GameObject threatIndicatorGo = AnimatedObjsManager.aom.SpawnAnimatedObj(indicatorConfig).gameObject;
            _activeThreatProyectileIndicator = threatIndicatorGo;
        }

        if(_uiBulletTimeIndicator != null)
        {
            _activeUiBulletTimeIndicator.SetActive(true);
        }

        if(_bulletTimePlayerMaterial.material != null)
        {
            PlayerControl.pc.PlayerMaterialManager.SetMaterialOverride(_bulletTimePlayerMaterial);
        }

        _activeTimescaleChange = TimescaleManager.tm.AddTimescaleChange(new(_timeScaleDecreaseOnBulletTime, true, _bulletTimeDuration, EndBulletTime));
    }
    void EndBulletTime()
    {
        _inBulletTime = false;
        _activeUiBulletTimeIndicator.SetActive(false);
        GameObject.Destroy(_activeThreatProyectileIndicator);
        if (_bulletTimePlayerMaterial.material != null)
        {
            PlayerControl.pc.PlayerMaterialManager.UnsetMaterialOverride(_bulletTimePlayerMaterial);
        }

    }
    public override void RemoveBehaviour()
    {
        if (!_inBulletTime)
            return;
        TimescaleManager.tm.RemoveTimescaleChange(_activeTimescaleChange);
        if (_activeUiBulletTimeIndicator != null)
            GameObject.Destroy(_activeUiBulletTimeIndicator);
    }
}
