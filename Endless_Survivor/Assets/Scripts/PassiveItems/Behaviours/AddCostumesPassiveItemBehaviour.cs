using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AddCostumesPassiveItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] List<Costume> _addingCostumes;

    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var addCostumesOriginal = original as AddCostumesPassiveItemBehaviour;
        _addingCostumes = addCostumesOriginal._addingCostumes;
        behaviourManager.onPicked += AddCostumes;

    }
    void AddCostumes()
    {
        foreach(var costume in _addingCostumes)
        {
            PlayerControl.pc.CostumeManager.AddCostume(costume);

        }
    }
    public override void RemoveBehaviour()
    {
        foreach (var costume in _addingCostumes)
        {
            PlayerControl.pc.CostumeManager.RemoveCostume(costume);

        }
    }
}