using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPointerEnemyBehaviour : EnemyBehaviour
{
    [SerializeField] Color _pointerColor;
    [SerializeField] Sprite _pointerIcon;
    [SerializeField] bool _destroyPointerWhenPlayerIsClose;
    UIPointer _pointer;
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var addPointerOriginal = original as AddPointerEnemyBehaviour;
        _pointerColor = addPointerOriginal._pointerColor;
        _pointerIcon = addPointerOriginal._pointerIcon;
        _destroyPointerWhenPlayerIsClose = addPointerOriginal._destroyPointerWhenPlayerIsClose;
    }
    public override void Start()
    {
        base.Start();
        _pointer = GameUIManager.uiManager.PointerManager.AddPointer(EnemyControl.transform, _pointerColor, _pointerIcon, _destroyPointerWhenPlayerIsClose);
    }
    public override void OnDeath()
    {
        base.OnDeath();
        RemovePointer(null);
    }
    void RemovePointer(EnemyControl placeholder)
    {
        GameUIManager.uiManager.PointerManager.RemovePointer(_pointer);
    }
}
