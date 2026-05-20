using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemInteractionManager : MonoBehaviour
{
    ItemInteractionData[] _itemInteractionDatas;
    List<ItemInteraction> _activeInteractions = new();

    private void Start()
    {
        _itemInteractionDatas = Resources.LoadAll<ItemInteractionData>("").ToArray();
        PlayerControl.pc.PassiveItemManager.OnItemPickup += CheckForFullfiledInteractions;
        PlayerControl.pc.PassiveItemManager.OnItemRemoved += CheckForUnfullfiledInteractions;
    }
    void CheckForFullfiledInteractions()
    {
        var heldItems = PlayerControl.pc.PassiveItemManager.PassiveItems;
        foreach (var itemInteraction in _itemInteractionDatas)
        {
            //if(alreadyFullfiled)
            //continue;
            var interactingWeapons = itemInteraction.GetInteractingItems(heldItems);
            if (_activeInteractions.Any(interaction => interaction.InteractionData == itemInteraction) || interactingWeapons.Count == 0)
                continue;
            AddInteractions(itemInteraction, interactingWeapons);
        }
    }
    void CheckForUnfullfiledInteractions()
    {
        var heldItems = PlayerControl.pc.PassiveItemManager.PassiveItems;
        List<ItemInteraction> activeInteractionsCopy = new(_activeInteractions);
        foreach (var interaction in activeInteractionsCopy)
        {
            if (interaction.InteractionData.GetInteractingItems(heldItems).Count != 0)
                continue;

            interaction.InteractionEnd();
            _activeInteractions.Remove(interaction);
        }
    }
    void AddInteractions(ItemInteractionData interactionData, List<PassiveItem> interactingItems)
    {
        List<ItemInteraction> addedInteractions = new();
        foreach (var interactionBehaviour in interactionData.InteractionBeahviours)
        {
            ItemInteraction newInteraction = Activator.CreateInstance(interactionBehaviour.GetType()) as ItemInteraction;
            newInteraction.Initialize(interactionBehaviour, interactionData, interactingItems);
            addedInteractions.Add(newInteraction);
        }
        foreach (var interaction in addedInteractions)
        {
            interaction.InteractionStart();
        }
        _activeInteractions.AddRange(addedInteractions);
        foreach (var item in interactingItems)
        {
            if (interactionData.ItemsNeededForInteraction.Find(x => x.itemNeededForTheInteraction == item.ItemData).isDestroyed)
                PlayerControl.pc.PassiveItemManager.RemovePassiveItem(item);
        }
    }
}
