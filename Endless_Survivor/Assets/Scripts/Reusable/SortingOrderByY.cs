using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrderByY : MonoBehaviour
{
    //[SerializeField] int _sortingOrder;
    [SerializeField] SpriteRenderer[] _affectedRenderers;
    //static readonly int _minOrder = -15;
    //public static int MinOrder {  get { return _minOrder; } }
    //public int SortingOrder { set { _sortingOrder = value; } }
    public SpriteRenderer[] AffectedRenderers {  set { _affectedRenderers = value; } }

    private void LateUpdate()
    {
        //float distY = PlayerControl.pc.transform.position.y - transform.position.y;
        //if(distY < 0)
        //{
        //    foreach(var renderer in _affectedRenderers)
        //        renderer.sortingOrder = _minOrder + _sortingOrder;
        //}
        //else
        //{
        //    foreach(var renderer in _affectedRenderers)
        //        renderer.sortingOrder = PlayerControl.pc.MainRenderer.sortingOrder + _sortingOrder;

        //}

        foreach (var renderer in _affectedRenderers)
            renderer.sortingOrder = (int)(-renderer.transform.position.y * 100);
    }
}
