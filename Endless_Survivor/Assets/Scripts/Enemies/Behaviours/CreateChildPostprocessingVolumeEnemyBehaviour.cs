using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class CreateChildPostprocessingVolumeEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] Vector2 _volumeOffset;
    [SerializeField] bool _onlyOnActive;
    [SerializeField] bool _isGlobal;
    [SerializeField] float _colliderRadius;
    [SerializeField] float _blendDistance;
    [Range(0,1)][SerializeField] float _weight;
    [SerializeField] int _priority;
    [SerializeField] VolumeProfile _volumeProfile;
    Volume _createdVolume;

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var createVolumeOriginal = original as CreateChildPostprocessingVolumeEnemyBehaviour;
        _volumeOffset = createVolumeOriginal._volumeOffset;
        _onlyOnActive = createVolumeOriginal._onlyOnActive;
        _isGlobal = createVolumeOriginal._isGlobal;
        _colliderRadius = createVolumeOriginal._colliderRadius;
        _blendDistance = createVolumeOriginal._blendDistance;
        _weight = createVolumeOriginal._weight;
        _priority = createVolumeOriginal._priority;
        _volumeProfile = createVolumeOriginal._volumeProfile;


    }

    public override void Start()
    {
        base.Start();
        GameObject createdVolumeObj = new(EnemyData.name + " Volume");
        createdVolumeObj.layer = LayerMask.NameToLayer("PostProcessing");

        _createdVolume = createdVolumeObj.AddComponent<Volume>();
        _createdVolume.transform.SetParent(EnemyControl.transform);
        _createdVolume.transform.localPosition = _volumeOffset;
        
        _createdVolume.isGlobal = _isGlobal;
        _createdVolume.weight = _weight;
        _createdVolume.priority = _priority;
        _createdVolume.profile = _volumeProfile;
        if (_isGlobal)
            return;
        var col = createdVolumeObj.AddComponent<SphereCollider>();
        col.radius = _colliderRadius;
        col.isTrigger = true;
        
        _createdVolume.blendDistance = _blendDistance;
    }
    public override void PassiveUpdate()
    {
        base.PassiveUpdate();
        if (!_onlyOnActive || _createdVolume.gameObject.activeSelf == IsActive)
            return;
        _createdVolume.gameObject.SetActive(IsActive);
    }
}
