using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class HorizontalLayoutGroupHugContent : MonoBehaviour
{
    [SerializeField] float _minSize;
    HorizontalLayoutGroup _horizontal;
    RectTransform _rectTransform;
    List<RectTransform> _children;

    private void OnEnable()
    {
        _horizontal = GetComponent<HorizontalLayoutGroup>();
        _rectTransform = GetComponent<RectTransform>();
        _children = GetComponentsInChildren<RectTransform>().Where(x => x.parent == _rectTransform).ToList();
    }
    void Update()
    {
        if (_children.Count == 0)
            return;
        float totalSize = _horizontal.spacing * (_children.Count - 1);
        foreach (var child in _children)
        {
            totalSize += child.sizeDelta.x;
        }

        _rectTransform.sizeDelta = new(totalSize > _minSize ? totalSize : _minSize, _rectTransform.sizeDelta.y);

        float updatedPosition = 0;
        if(_horizontal.childAlignment == TextAnchor.UpperLeft || _horizontal.childAlignment == TextAnchor.MiddleLeft || _horizontal.childAlignment == TextAnchor.LowerLeft)
                updatedPosition = _rectTransform.sizeDelta.x / 2;
        if(_horizontal.childAlignment == TextAnchor.UpperRight || _horizontal.childAlignment == TextAnchor.MiddleRight || _horizontal.childAlignment == TextAnchor.LowerRight)
                updatedPosition = _minSize - _rectTransform.sizeDelta.x / 2;

        _rectTransform.localPosition = Vector3.right * updatedPosition + Vector3.up * _rectTransform.position.y;
    }
    private void OnTransformChildrenChanged()
    {
        _children = GetComponentsInChildren<RectTransform>().Where(x => x.parent == _rectTransform).ToList();
    }
}
