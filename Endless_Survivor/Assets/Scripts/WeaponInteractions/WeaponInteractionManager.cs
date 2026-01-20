using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponInteractionManager : MonoBehaviour
{
    WeaponInteractionData[] _weaponInteractionDatas;
    List<WeaponInteraction> _activeInteractions = new();

    private void Start()
    {
        _weaponInteractionDatas = Resources.LoadAll<WeaponInteractionData>("").ToArray();
        PlayerControl.pc.WeaponManager.OnWeaponPickup += CheckForFullfiledInteractions;
        PlayerControl.pc.WeaponManager.OnWeaponRemoved += CheckForUnfullfiledInteractions;
    }
    void CheckForFullfiledInteractions()
    {
        var heldWeapons = PlayerControl.pc.WeaponManager.HeldWeapons;
        foreach (var weaponInteraction in _weaponInteractionDatas)
        {
            //if(alreadyFullfiled)
            //continue;
            var interactingWeapons = weaponInteraction.GetInteractingWeapons(heldWeapons);
            if (_activeInteractions.Any(interaction => interaction.InteractionData == weaponInteraction) || interactingWeapons.Count == 0)
                continue;
            AddInteractions(weaponInteraction, interactingWeapons);
        }
    }
    void CheckForUnfullfiledInteractions()
    {
        var heldWeapons = PlayerControl.pc.WeaponManager.HeldWeapons;
        List<WeaponInteraction> activeInteractionsCopy = new(_activeInteractions);
        foreach(var interaction in activeInteractionsCopy)
        {
            if (interaction.InteractionData.GetInteractingWeapons(heldWeapons).Count != 0)
                continue;

            interaction.InteractionEnd();
            _activeInteractions.Remove(interaction);
        }
    }
    void AddInteractions(WeaponInteractionData interactionData, List<WeaponAttackManager> interactingWeapons)
    {
        List<WeaponInteraction> addedInteractions = new();
        print("ADDING " + interactionData.name);
        foreach(var interactionBehaviour in interactionData.InteractionBeahviours)
        {
            WeaponInteraction newInteraction = Activator.CreateInstance(interactionBehaviour.GetType()) as WeaponInteraction;
            newInteraction.Initialize(interactionBehaviour, interactionData, interactingWeapons);
            addedInteractions.Add(newInteraction);
        }
        foreach(var interaction in addedInteractions)
        {
            interaction.InteractionStart();
        }
        _activeInteractions.AddRange(addedInteractions);
        foreach(var weapon in interactingWeapons)
        {
            if(interactionData.WeaponsNeededForInteraction.Find(x => x.weaponNeededForTheInteraction ==  weapon.WeaponData).isDestroyed)
                PlayerControl.pc.WeaponManager.RemoveWeapon(weapon);
        }
    }
}
