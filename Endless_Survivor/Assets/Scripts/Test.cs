using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] Sprite _sprite;
    [SerializeField] Transform _target;
    private void Start()
    {
        GameUIManager.uiManager.PointerManager.AddPointer(_target, Color.white, _sprite);
    }
    private void Update()
    {
        Debug.Log(Camera.main.WorldToScreenPoint(_target.position));
        
    }
}