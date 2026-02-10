using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilledBarRenderer : MonoBehaviour
{
    [SerializeField] SpriteRenderer _fill;
    Vector2 _initialScale;
    Vector2 _initialPos;

    private void Start()
    {
        _initialScale = _fill.transform.localScale;
        _initialPos = _fill.transform.localPosition;
    }
    public void SetFillValue(float xValue, float yValue)
    {
        xValue = Mathf.Clamp01(xValue);
        yValue = Mathf.Clamp01(yValue);

        float xScaleChange = Mathf.Lerp(0, _initialScale.x, xValue);
        float yScaleChange = Mathf.Lerp(0, _initialScale.y, yValue);

        _fill.transform.localScale = new(xScaleChange, yScaleChange, 1);
        _fill.transform.localPosition = new(_initialPos.x - (_initialScale.x - xScaleChange) / 2, _initialPos.y - (_initialScale.y - yScaleChange) / 2, 0);
    }
}
