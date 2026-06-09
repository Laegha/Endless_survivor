using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AddCostumesOnActivatePassiveItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] List<Costume> _addingCostumes;
    [SerializeField] bool _removeCostumesAfterTime;
    [SerializeField] float _timeToRemoveCostumes;

    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var addCostumesOriginal = original as AddCostumesOnActivatePassiveItemBehaviour;
        _addingCostumes = addCostumesOriginal._addingCostumes;
        _removeCostumesAfterTime = addCostumesOriginal._removeCostumesAfterTime;
        _timeToRemoveCostumes = addCostumesOriginal._timeToRemoveCostumes;
    }

    public override void Activate()
    {
        base.Activate();
        AddCostumes();
    }

    void AddCostumes()
    {
        foreach (var costume in _addingCostumes)
        {
            PlayerControl.pc.CostumeManager.AddCostume(costume);

        }
        if (_removeCostumesAfterTime)
            GameManager.gm.DelayAction(_timeToRemoveCostumes, RemoveCostumes, () => BehaviourManager == null);
    }
    void RemoveCostumes()
    {
        foreach (var costume in _addingCostumes)
        {
            PlayerControl.pc.CostumeManager.RemoveCostume(costume);

        }
    }
    public override void RemoveBehaviour()
    {
        RemoveCostumes();
    }
}
