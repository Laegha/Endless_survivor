using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AddCostumesPassiveItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] List<Costume> _addingCostume;

    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var addCostumesOriginal = original as AddCostumesPassiveItemBehaviour;
        _addingCostume = addCostumesOriginal._addingCostume;
        behaviourManager.onPicked += AddCostume;

    }
    void AddCostume()
    {
        foreach(var costume in _addingCostume)
        {
            PlayerControl.pc.CostumeManager.AddCostume(costume);

        }
    }
    public override void RemoveBehaviour()
    {
        foreach (var costume in _addingCostume)
        {
            PlayerControl.pc.CostumeManager.RemoveCostume(costume);

        }
    }
}
