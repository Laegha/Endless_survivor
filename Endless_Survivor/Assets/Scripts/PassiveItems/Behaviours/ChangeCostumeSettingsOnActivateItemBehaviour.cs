using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCostumeSettingsOnActivateItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] CharacterCostumeSettings _newSettings;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var changeCostumeSettingsOriginal = original as ChangeCostumeSettingsOnActivateItemBehaviour;
        _newSettings = changeCostumeSettingsOriginal._newSettings;
    }
    public override void Activate()
    {
        base.Activate();
        PlayerControl.pc.CostumeManager.ChangeCostumeSettings(_newSettings);
    }
    public override void RemoveBehaviour()
    {

    }
}
