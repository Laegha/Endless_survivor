using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyInvokationInfo
{
    [SerializeField] EnemyData _invokedEnemy;
    [SerializeField] Color _pointerColor;
    [SerializeField] Sprite _pointerIcon;
    bool _isBoss;

    public EnemyData InvokedEnemy {  get { return _invokedEnemy; } }
    public Color PointerColor { get { return _pointerColor; } }
    public Sprite PointerIcon { get { return _pointerIcon; } }
    public bool IsBoss { get { return _isBoss; } }

    public EnemyInvokationInfo(EnemyData invokedEnemy, Color pointerColor, Sprite pointerIcon, bool isBoss)
    {
        _invokedEnemy = invokedEnemy;
        _pointerColor = pointerColor;
        _pointerIcon = pointerIcon;
        _isBoss = isBoss;
    }
}
