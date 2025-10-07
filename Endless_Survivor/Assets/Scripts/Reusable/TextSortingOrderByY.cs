using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextSortingOrderByY : SortingOrderByY<TextMeshPro>
{
    public override void LateUpdate()
    {
        base.LateUpdate();

        foreach (var elemOrder in ElementsOrders)
            elemOrder.Key.sortingOrder = elemOrder.Value;
    }
}
