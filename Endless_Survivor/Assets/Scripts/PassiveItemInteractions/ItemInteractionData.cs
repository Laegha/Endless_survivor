using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemInteractionData : ScriptableObject
{
    [SerializeField] List<ItemInteractionItemInfo> _itemsNeededForInteraction;
    [SerializeReference] List<ItemInteraction> _interactionsBehaviours = new();

    public List<ItemInteractionItemInfo> ItemsNeededForInteraction { get { return _itemsNeededForInteraction; } }
    public List<ItemInteraction> InteractionBeahviours { get { return _interactionsBehaviours; } }
    public List<PassiveItem> GetInteractingItems(List<PassiveItem> heldItems)
    {
        List<PassiveItem> result = new List<PassiveItem>();
        foreach (var neededItem in _itemsNeededForInteraction)
        {
            var matchingHeldItems = heldItems.Where(passiveItem => passiveItem.ItemData == neededItem.itemNeededForTheInteraction);
            if (matchingHeldItems.Count() < neededItem.neededCount)
                return new();

            result.AddRange(matchingHeldItems);

        }
        return result;
    }
}
