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

    public EnemyData InvokedEnemy {  get { return _invokedEnemy; } }
    public Color PointerColor { get { return _pointerColor; } }
    public Sprite PointerIcon { get { return _pointerIcon; } }
}
