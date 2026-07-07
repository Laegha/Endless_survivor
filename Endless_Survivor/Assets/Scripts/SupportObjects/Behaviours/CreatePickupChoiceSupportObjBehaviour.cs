using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CreatePickupChoiceSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] List<RouletteElementChance<PickupData>> _options;
    [SerializeField] int _optionsAmmount;
    [SerializeField] bool _allowRepetitions;
    [SerializeReference] IPattern _pickupsSpawnPattern;
    [SerializeField] Vector2 _patternCenterOffset;
    [SerializeField] ParticleSystem _unchosenPickupsParticles;

    List<GameObject> _createdPickups = new();
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var createPickupOriginal = original as CreatePickupChoiceSupportObjBehaviour;
        _options = new(createPickupOriginal._options);
        _optionsAmmount = createPickupOriginal._optionsAmmount;
        _allowRepetitions = createPickupOriginal._allowRepetitions;
        _pickupsSpawnPattern = createPickupOriginal._pickupsSpawnPattern;
        _patternCenterOffset = createPickupOriginal._patternCenterOffset;
        _unchosenPickupsParticles = createPickupOriginal._unchosenPickupsParticles;

        OnStart += CreatePickups;
    }

    void CreatePickups()
    {
        List<Vector2> pickupPositions = _pickupsSpawnPattern.GetPositions((Vector2)ObjControl.transform.position +  _patternCenterOffset, _optionsAmmount).ToList();
        List<RouletteElementChance<PickupData>> availableOptions = new(_options);
        for (int i = 0; i < _optionsAmmount; i++)
        {
            if (availableOptions.Count == 0)
                break;
            PickupData creatingPickup = Utility.GetRouletteElement(availableOptions);
            var pickup = Utility.GeneratePickup(creatingPickup, pickupPositions[i]);
            _createdPickups.Add(pickup.gameObject);
            pickup.Pickup.OnPickup += EndChoice;
            if (!_allowRepetitions)
                availableOptions.RemoveAll(x => x.RouletteElement == creatingPickup);
        }
    }

    void EndChoice(Pickup pickedUp)
    {
        foreach(var pickup in _createdPickups)
        {
            if (pickup == pickedUp.gameObject)
                continue;
            ObjectDestroyingManager.odm.DestroyObj(pickup.gameObject, null, 0);
            ParticleConfig particlesConfig = new(_unchosenPickupsParticles, pickup.transform.position, Quaternion.identity, _unchosenPickupsParticles.main.duration, null, false, false);
            ParticleManager.pm.SpawnParticles(particlesConfig);
        }
        ObjectDestroyingManager.odm.DestroyObj(ObjControl.gameObject, null, 0);
    }
}
