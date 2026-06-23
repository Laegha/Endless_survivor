using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class ModularPanel : MonoBehaviour
{
    [SerializeField] ModularPanelData _panelData;
    List<GameObject> _currentRows = new List<GameObject>();

    RectTransform _panelTransform;
    
    Vector2 _currSize;
    private void Awake()
    {
        _panelTransform = GetComponent<RectTransform>();
        var children = GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            if(child == transform) continue;
            _currentRows.Add(child.gameObject);
        }
    }
    void Update()
    {
        if (_panelData == null /*|| !_panelData.IsUsable()*/)
            return;
        var prevSize = _currSize;
        _currSize = _panelTransform.sizeDelta;
        if (prevSize == _currSize)
            return;


        UpdatePanel();
    }

    void UpdatePanel()
    {
        foreach(GameObject row in _currentRows)
        {
            if (row == transform)
                continue;
            DestroyImmediate(row.gameObject);

        }
        Vector2 panelSize = _panelTransform.sizeDelta;
        int columns = (int)(panelSize.x / _panelData.CellSize.x);
        int rows = (int)(panelSize.y / _panelData.CellSize.y);

        for(int i = 0; i < rows; i++)
        {
            GameObject row = Instantiate(_panelData.RowPrefab, transform);
            _currentRows.Add(row);
            row.GetComponent<RectTransform>().sizeDelta = new(_currSize.x, _panelData.CellSize.y);
            for(int j = 0; j < columns; j++)
            {
                GameObject cell = Instantiate(_panelData.CellPrefab, row.transform);
                var cellSprite = _panelData.GetCell(i, j, rows-1, columns-1);
                var cellImage = cell.GetComponentInChildren<Image>();
                cellImage.sprite = cellSprite;
                cell.GetComponent<RectTransform>().sizeDelta = new(_panelData.CellSize.x, _panelData.CellSize.y);
                if (cellSprite == null)
                    cellImage.color = new(255, 255, 255, 0);
            }
        }
    }
}
