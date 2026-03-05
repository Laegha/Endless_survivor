using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CreateRandomSupportObjsOnDestroySupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] List<RouletteElementChance<SupportObjectData>> _possibleSupportObjs;
    [SerializeField] RandomBetweenTwoConstants _ammount;
    [SerializeReference] IPattern _creationPattern;
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var createSupportObjOriginal = original as CreateRandomSupportObjsOnDestroySupportObjBehaviour;
        _possibleSupportObjs = new(createSupportObjOriginal._possibleSupportObjs);
        _ammount = createSupportObjOriginal._ammount;
        _creationPattern = createSupportObjOriginal._creationPattern;
        OnDestroyed += CreateRandomObjs;
    }

    void CreateRandomObjs()
    {
        int createdAmmount = (int)_ammount.rand;
        var createdObjsPositions = _creationPattern.GetPositions(ObjControl.transform.position, createdAmmount).ToList();
        for(int i = 0; i < createdAmmount; i++)
        {
            SupportObjectData generatedObj = Utility.GetRouletteElement(_possibleSupportObjs);
            Utility.GenerateSupportObj(generatedObj, createdObjsPositions[i], Quaternion.identity);

        }

    }
}
