using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PanelData", menuName = "ScriptableObjects/UI/Panel", order = 0)]
public class ModularPanelData : ScriptableObject
{
    [SerializeField] GameObject _rowPrefab;
    [SerializeField] GameObject _cellPrefab;

    [SerializeField] Vector2 _cellSize;
    [SerializeField] Sprite _upLeftPanel;
    [SerializeField] Sprite _upPanel;
    [SerializeField] Sprite _upRightPanel;
    [SerializeField] Sprite _leftPanel;
    [SerializeField] Sprite _middlePanel;
    [SerializeField] Sprite _rightPanel;
    [SerializeField] Sprite _downLeftPanel;
    [SerializeField] Sprite _downPanel;
    [SerializeField] Sprite _downRightPanel;
    public GameObject RowPrefab { get { return _rowPrefab; } }
    public GameObject CellPrefab { get { return _cellPrefab; } }
    public Vector2 CellSize { get { return _cellSize; } }
    Sprite[] _panels => new Sprite[9] { _upLeftPanel, _upPanel, _upRightPanel, _rightPanel, _middlePanel, _leftPanel, _downLeftPanel, _downPanel, _downRightPanel };
    public bool IsUsable()
    {
        if(_rowPrefab ==  null || _cellPrefab == null)
            return false;
        foreach(var panel in _panels)
            if(panel == null)
                return false;
        return true;
    }
    public Sprite GetCell(int cellRow, int cellCol, int rows, int cols)
    {
        Sprite resultPanel = null;
        if (cellRow == 0)
        {
            if (cellCol == 0)
                resultPanel = _upLeftPanel;
            else if (cellCol == cols)
                resultPanel = _upRightPanel;
            else
                resultPanel = _upPanel;
        }
        else if (cellRow == rows)
        {
            if (cellCol == 0)
                resultPanel = _downLeftPanel;
            else if (cellCol == cols)
                resultPanel = _downRightPanel;
            else
                resultPanel = _downPanel;
        }
        else
        {
            if (cellCol == 0)
                resultPanel = _leftPanel;
            else if (cellCol == cols)
                resultPanel = _rightPanel;
            else
                resultPanel = _middlePanel;
        }
        return resultPanel;
    }
}
