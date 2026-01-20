using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponInteraction", menuName = "ScriptableObjects/WeaponInteraction", order = 3)]
public class WeaponInteractionData : ScriptableObject
{
    [SerializeField] List<WeaponInteractionWeaponInfo> _weaponsNeededForInteraction;
    [SerializeReference] List<WeaponInteraction> _interactionsBehaviours = new();

    public List<WeaponInteractionWeaponInfo> WeaponsNeededForInteraction { get {  return _weaponsNeededForInteraction; } }
    public List<WeaponInteraction> InteractionBeahviours { get { return _interactionsBehaviours; } }
    public List<WeaponAttackManager> GetInteractingWeapons(List<WeaponAttackManager> heldWeapons)
    {
        List<WeaponAttackManager> result = new List<WeaponAttackManager>();   
        foreach(var neededWeapon in _weaponsNeededForInteraction)
        {
            var matchingHeldWeapons = heldWeapons.Where(weaponControl => weaponControl.WeaponData == neededWeapon.weaponNeededForTheInteraction);
            if(matchingHeldWeapons.Count() < neededWeapon.neededCount) 
                return new();

            result.AddRange(matchingHeldWeapons);

        }
        return result;
    }
}
