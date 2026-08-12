using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRow
{
    ObjectRowData _rowData;
    Func<Vector2> _rowStartPos;
    List<ObjectRowElement> _rowObjs = new();
    public List<ObjectRowElement> RowObjs {  get { return _rowObjs; } }
    public ObjectRow(ObjectRowData rowData, Func<Vector2> rowStartPos)
    {
        _rowData = rowData;
        _rowStartPos = rowStartPos;

    }
    public ObjectRowElement AddObj(GameObject obj)
    {
        ObjectRowElement newElem = new ObjectRowElement(obj.transform, obj.transform.position);
        _rowObjs.Add(newElem);
        return newElem;
    }

    public void UpdateObjsPositions()
    {
        List<ObjectRowElement> rowObjsCopy = new(_rowObjs);
        int objIndex = 0;
        foreach (var obj in rowObjsCopy)
        {
            if (obj == null || obj.ElementTr == null)
            {
                _rowObjs.Remove(obj);
                continue;
            }
            obj.PrevPos = obj.ElementTr.position;
            Vector2 newPos = _rowStartPos() + _rowData.GetObjRowPos(objIndex);
            obj.ElementTr.position = newPos;
            objIndex++;
        }
    }
}