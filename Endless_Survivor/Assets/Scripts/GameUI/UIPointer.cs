using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class UIPointer : MonoBehaviour 
{
    [SerializeField] Image _bgImage;
    [SerializeField] Image _iconImage;
    [SerializeField] RectTransform _iconImageTarget;
    RectTransform _pointerTr;
    Transform _playerTr;
    Transform _targetTr;
    RectTransform _iconTr;

    Vector2 _canvasSize = Vector2.one;
    Vector2 _showingLimits;

    public void SetValues(Transform playerTr, Transform targetTr, Color bgColor, Sprite icon)
    {
        _canvasSize = transform.root.GetComponent<RectTransform>().sizeDelta;
        _pointerTr = GetComponent<RectTransform>();
        _playerTr = playerTr;
        _targetTr = targetTr;
        _bgImage.color = bgColor;
        _iconImage.sprite = icon;
        _iconTr = _iconImage.GetComponent<RectTransform>();
        Utility.ScaleImageToFitTarget(_iconTr, icon, _iconImageTarget.sizeDelta);

        float cameraSize = Camera.main.orthographicSize;
        float limitsY = 0;
        float limitsX = 0;
        if(_canvasSize.y > _canvasSize.x)
        {
            limitsY = cameraSize;
            limitsX = (_canvasSize.x / _canvasSize.y) * cameraSize;
        }
        else
        {
            limitsY = (_canvasSize.y / _canvasSize.x) * cameraSize;
            limitsX = cameraSize;

        }

        _showingLimits = new Vector2(limitsX, limitsY);
    }

    public void Update()
    {
        Vector2 dir = (_targetTr.position - _playerTr.position).normalized;


        Vector2 halfSize = _canvasSize * 0.5f;

        float scaleX = Mathf.Abs(halfSize.x / dir.x);
        float scaleY = Mathf.Abs(halfSize.y / dir.y);

        float scale = Mathf.Min(scaleX, scaleY);

        Vector2 pointerPos = dir * scale;

        _pointerTr.anchoredPosition = pointerPos;
        float angle = Mathf.Atan2(dir.y, dir.x);
        _pointerTr.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg); 
        _iconTr.rotation = Quaternion.Euler(0, 0, -angle * Mathf.Rad2Deg);
    }
}
