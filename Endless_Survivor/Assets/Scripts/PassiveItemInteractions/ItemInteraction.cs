using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction
{
    public static int maxStacks => 0;
    ItemInteractionData _interactionData;
    List<PassiveItem> _affectedItems;

    public ItemInteractionData InteractionData { get { return _interactionData; } }

    public virtual void Initialize(ItemInteraction original, ItemInteractionData interactionData, List<PassiveItem> affectedItems)
    {
        _affectedItems = new List<PassiveItem>(affectedItems);
        _interactionData = interactionData;
    }
    public virtual void InteractionStart()
    {

    }
    public virtual void InteractionEnd()
    {

    }
}
