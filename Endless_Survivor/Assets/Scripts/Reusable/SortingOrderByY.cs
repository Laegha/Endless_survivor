using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrderByY<T> : MonoBehaviour where T : Component
{
    [SerializeField] T[] sortedElements;
    [SerializeField] int _offset;
    static SortingOrderByY<T> _highestSorter;
    public static int highestOrder;
    Dictionary<T, int> _elementsOrders = new();
    public T[] AffectedRenderers {  set { sortedElements = value; } }
    public Dictionary<T, int> ElementsOrders { get { return _elementsOrders; } }
    private void Start()
    {
        foreach(var element in sortedElements) 
            _elementsOrders.Add(element, 0);
    }

    public virtual void LateUpdate()
    {
        int thisHighestOrder = 0;
        bool sortingOrderDefined = false;
        foreach (var sortedElement in sortedElements)
        {
            int elementOrder = (int)(-sortedElement.transform.position.y * 100) + _offset;
            _elementsOrders[sortedElement] = elementOrder;
            if(!sortingOrderDefined || thisHighestOrder < elementOrder)
            {
                thisHighestOrder = elementOrder;
                sortingOrderDefined = true;
            }
        }

        if (thisHighestOrder < highestOrder && this != _highestSorter)
            return;
        highestOrder = thisHighestOrder;
        _highestSorter = this;
    }
}
