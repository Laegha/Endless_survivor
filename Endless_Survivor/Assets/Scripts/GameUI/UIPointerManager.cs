using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPointerManager : MonoBehaviour
{
    [SerializeField] GameObject _pointerPrefab;
    List<UIPointer> _activePointers = new();
    public UIPointer AddPointer(Transform target, Color pointerColor, Sprite pointerIcon)
    {
        UIPointer addedPointer = GameObject.Instantiate(_pointerPrefab, transform).GetComponent<UIPointer>();
        
        addedPointer.SetValues(PlayerControl.pc.transform, target, pointerColor, pointerIcon);
        _activePointers.Add(addedPointer);
        return addedPointer;

    }

    public void RemovePointer(UIPointer removedPointer)
    {
        _activePointers.Remove(removedPointer);
        Destroy(removedPointer.gameObject);
    }

    private void Update()
    {
        foreach(var pointer in _activePointers)
        {
            pointer.Update();
        }
                
    }
}
