using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateConstantlyRectTransform : MonoBehaviour
{
    [SerializeField] float _rotateSpeed;
    RectTransform _rectTransform;
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        _rectTransform.Rotate(new(0,0,_rotateSpeed * Time.deltaTime));
    }
}
