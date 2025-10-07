using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererSortingByY : SortingOrderByY<SpriteRenderer>
{
    public override void LateUpdate()
    {
        base.LateUpdate();

        foreach(var elemOrder in ElementsOrders)
            elemOrder.Key.sortingOrder = elemOrder.Value;
    }
}
